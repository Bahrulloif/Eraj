using AutoMapper;
using Domain.DTOs.CartDTO;
using Domain.Entities;
using Domain.Filters.CartFilter;
using Domain.Responses;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.CartService;

public class CartService : ICartService
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;
    public CartService(DataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    // Unlike Address/Order/Profile (which have a real "privileged sees everyone's" use case - an
    // admin panel that manages other users' records), Cart is purely personal and has no such
    // screen anywhere in the app. Bypassing the owner filter here for isPrivileged meant a
    // SuperAdmin/Admin's own "Корзина" page silently showed every user's cart rows mixed into
    // their own - found live (frontend/speca.md's Фаза 1-3 real-browser report, 2026-09-11):
    // opening it as SuperAdmin showed 2 stray rows that actually belonged to a test account.
    // Always scope the list to the caller now, privileged or not. GetCartById/UpdateCart/
    // DeleteCart below keep their isPrivileged bypass - those require already knowing a specific
    // cart row's id (support/debug lookup of one record), not a blanket dump of every cart.
    public async Task<Response<List<GetCartDTO>>> GetCart(CartFilter filter, string currentUserId, bool isPrivileged)
    {
        var query = _context.Carts.Where(c => c.ApplicationUserId == currentUserId);
        if (filter.Id != 0)
        {
            query = query.Where(c => c.Id == filter.Id);
        }
        var result = await query.ToListAsync();
        var response = _mapper.Map<List<GetCartDTO>>(result);
        return new Response<List<GetCartDTO>>(response);
    }
    public async Task<Response<GetCartDTO>> GetCartById(int cartId, string currentUserId, bool isPrivileged)
    {
        var find = await _context.Carts.FirstOrDefaultAsync(c => c.Id == cartId);
        if (find == null)
        {
            return new Response<GetCartDTO>(System.Net.HttpStatusCode.NotFound, "Cart Not Found");
        }
        if (!isPrivileged && find.ApplicationUserId != currentUserId)
        {
            return new Response<GetCartDTO>(System.Net.HttpStatusCode.Forbidden, "You do not have access to this cart");
        }
        var mapped = _mapper.Map<GetCartDTO>(find);
        return new Response<GetCartDTO>(mapped);
    }

    public async Task<Response<string>> AddCart(AddCartDTO cart, string currentUserId)
    {
        // Always the caller's own cart — never trust a client-supplied ApplicationUserId.
        cart.ApplicationUserId = currentUserId;
        var find = await _context.Carts.AsNoTracking().FirstOrDefaultAsync(c => c.Id == cart.Id);
        if (find == null)
        {
            var mapped = _mapper.Map<Cart>(cart);
            await _context.Carts.AddAsync(mapped);
            await _context.SaveChangesAsync();
            return new Response<string>("Cart added successfully");
        }
        return new Response<string>("Cart already exist");
    }

    public async Task<Response<string>> UpdateCart(AddCartDTO cart, string currentUserId, bool isPrivileged)
    {
        var find = await _context.Carts.FirstOrDefaultAsync(c => c.Id == cart.Id);
        if (find == null)
        {
            return new Response<string>(System.Net.HttpStatusCode.NotFound, "Cart Not Found");
        }
        if (!isPrivileged && find.ApplicationUserId != currentUserId)
        {
            return new Response<string>(System.Net.HttpStatusCode.Forbidden, "You do not have access to this cart");
        }
        if (!isPrivileged)
        {
            // Never let a non-privileged caller reassign the cart to someone else.
            cart.ApplicationUserId = currentUserId;
        }
        _mapper.Map(cart, find);
        _context.Carts.Update(find);
        await _context.SaveChangesAsync();
        return new Response<string>("Cart updated successfully");
    }
    public async Task<Response<GetCartDTO>> DeleteCart(int cartId, string currentUserId, bool isPrivileged)
    {
        var find = await _context.Carts.FirstOrDefaultAsync(c => c.Id == cartId);
        if (find == null)
        {
            return new Response<GetCartDTO>(System.Net.HttpStatusCode.NotFound, "Cart Not Found");
        }
        if (!isPrivileged && find.ApplicationUserId != currentUserId)
        {
            return new Response<GetCartDTO>(System.Net.HttpStatusCode.Forbidden, "You do not have access to this cart");
        }
        _context.Carts.Remove(find);
        await _context.SaveChangesAsync();
        return new Response<GetCartDTO>(System.Net.HttpStatusCode.OK, "Cart deleted successfully");
    }
}
