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
    public async Task<Response<List<GetCartDTO>>> GetCart(CartFilter filter)
    {
        var find = await _context.Carts.FirstOrDefaultAsync(c => c.Id == filter.Id);
        if (find != null)
        {
            var mapped = _mapper.Map<List<GetCartDTO>>(find);
            return new Response<List<GetCartDTO>>(mapped);
        }
        var result = await _context.Carts.ToListAsync();
        var response = _mapper.Map<List<GetCartDTO>>(result);
        return new Response<List<GetCartDTO>>(response);
    }
    public async Task<Response<GetCartDTO>> GetCartById(int cartId)
    {
        var find = await _context.Carts.FirstOrDefaultAsync(c => c.Id == cartId);
        if (find != null)
        {
            var mapped = _mapper.Map<GetCartDTO>(find);
            return new Response<GetCartDTO>(mapped);
        }
        return new Response<GetCartDTO>(System.Net.HttpStatusCode.NotFound, "Cart Not Found");
    }

    public async Task<Response<string>> AddCart(AddCartDTO cart)
    {
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

    public async Task<Response<string>> UpdateCart(AddCartDTO cart)
    {
        var find = await _context.Carts.AsNoTracking().FirstOrDefaultAsync(c => c.Id == cart.Id);
        if (find != null)
        {
            var mapped = _mapper.Map<Cart>(cart);
            _context.Carts.Update(mapped);
            await _context.SaveChangesAsync();
            return new Response<string>("Cart updated successfully");
        }
        return new Response<string>("Cart Not Found");
    }
    public async Task<Response<GetCartDTO>> DeleteCart(int cartId)
    {
        var find = await _context.Carts.FirstOrDefaultAsync(c => c.Id == cartId);
        if (find != null)
        {
            _context.Carts.Remove(find);
            await _context.SaveChangesAsync();
            return new Response<GetCartDTO>(System.Net.HttpStatusCode.OK, "Cart deleted successfully");
        }
        return new Response<GetCartDTO>(System.Net.HttpStatusCode.NotFound, "Cart Not Found");
    }
}
