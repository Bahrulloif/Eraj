using AutoMapper;
using Domain.DTOs.OrderDTO;
using Domain.Entities;
using Domain.Filters.OrderFilter;
using Domain.Responses;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.OrderService;

public class OrderService : IOrderService
{
    private readonly IMapper _mapper;
    private readonly DataContext _context;

    public OrderService(IMapper mapper, DataContext context)
    {
        _mapper = mapper;
        _context = context;
    }
    public async Task<Response<List<GetOrderDTO>>> GetOrder(OrderFilter filter, string currentUserId, bool isPrivileged)
    {
        var query = _context.Orders.AsQueryable();
        if (!isPrivileged)
        {
            query = query.Where(o => o.ApplicationUserId == currentUserId);
        }
        if (filter.Id != null)
        {
            query = query.Where(o => o.Id == filter.Id);
        }
        var orders = await query.ToListAsync();
        var result = _mapper.Map<List<GetOrderDTO>>(orders);
        return new Response<List<GetOrderDTO>>(result);
    }

    public async Task<Response<GetOrderDTO>> GetOrderById(int orderId, string currentUserId, bool isPrivileged)
    {
        var find = await _context.Orders.FirstOrDefaultAsync(o => o.Id == orderId);
        if (find == null)
        {
            return new Response<GetOrderDTO>(System.Net.HttpStatusCode.NotFound, "Order Not Found");
        }
        if (!isPrivileged && find.ApplicationUserId != currentUserId)
        {
            return new Response<GetOrderDTO>(System.Net.HttpStatusCode.Forbidden, "You do not have access to this order");
        }
        var mapped = _mapper.Map<GetOrderDTO>(find);
        return new Response<GetOrderDTO>(mapped);
    }
    public async Task<Response<GetOrderDTO>> AddOrder(AddOrderDTO order, string currentUserId)
    {
        // Always the caller's own order — never trust a client-supplied UserId.
        order.UserId = currentUserId;
        var find = await _context.Orders.FirstOrDefaultAsync(o => o.Id == order.Id);
        if (find == null)
        {
            var mapped = _mapper.Map<Order>(order);
            await _context.Orders.AddAsync(mapped);
            await _context.SaveChangesAsync();
            return new Response<GetOrderDTO>(System.Net.HttpStatusCode.OK, "Order added successfully");
        }
        return new Response<GetOrderDTO>(System.Net.HttpStatusCode.Conflict, "Order already exist");

    }

    public async Task<Response<GetOrderDTO>> UpdateOrder(AddOrderDTO order, string currentUserId, bool isPrivileged)
    {
        var find = await _context.Orders.FirstOrDefaultAsync(o => o.Id == order.Id);
        if (find == null)
        {
            return new Response<GetOrderDTO>(System.Net.HttpStatusCode.NotFound, "Order Not Found");
        }
        if (!isPrivileged && find.ApplicationUserId != currentUserId)
        {
            return new Response<GetOrderDTO>(System.Net.HttpStatusCode.Forbidden, "You do not have access to this order");
        }
        if (!isPrivileged)
        {
            // Never let a non-privileged caller reassign the order to someone else.
            order.UserId = currentUserId;
        }
        _mapper.Map(order, find);
        _context.Orders.Update(find);
        await _context.SaveChangesAsync();
        return new Response<GetOrderDTO>(System.Net.HttpStatusCode.OK, "Order updated successfully");
    }
    public async Task<Response<GetOrderDTO>> DeleteOrder(int orderId, string currentUserId, bool isPrivileged)
    {
        var find = await _context.Orders.FirstOrDefaultAsync(o => o.Id == orderId);
        if (find == null)
        {
            return new Response<GetOrderDTO>(System.Net.HttpStatusCode.NotFound, "Order Not Found");
        }
        if (!isPrivileged && find.ApplicationUserId != currentUserId)
        {
            return new Response<GetOrderDTO>(System.Net.HttpStatusCode.Forbidden, "You do not have access to this order");
        }
        _context.Orders.Remove(find);
        await _context.SaveChangesAsync();
        return new Response<GetOrderDTO>(System.Net.HttpStatusCode.OK, "Order deleted successfully");
    }
}
