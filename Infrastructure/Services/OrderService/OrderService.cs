using AutoMapper;
using Domain.DTOs.OrderDTO;
using Domain.Entities;
using Domain.Filters.OrderFilter;
using Domain.Responses;
using Infrastructure.Data;
using Microsoft.AspNetCore.Authentication;
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
    public async Task<Response<List<GetOrderDTO>>> GetOrder(OrderFilter filter)
    {
        if (filter.Id != null)
        {
            var orders = await _context.Orders.FirstOrDefaultAsync(o => o.Id == filter.Id);
            var mapped = _mapper.Map<List<GetOrderDTO>>(orders);
            return new Response<List<GetOrderDTO>>(mapped);
        }
        var find = await _context.Orders.ToListAsync();
        var result = _mapper.Map<List<GetOrderDTO>>(find);
        return new Response<List<GetOrderDTO>>(result);
    }

    public async Task<Response<GetOrderDTO>> GetOrderById(int orderId)
    {
        var find = await _context.Orders.FirstOrDefaultAsync(o => o.Id == orderId);
        if (find != null)
        {
            var mapped = _mapper.Map<GetOrderDTO>(find);
            return new Response<GetOrderDTO>(mapped);
        }
        return new Response<GetOrderDTO>(System.Net.HttpStatusCode.NotFound, "Order Not Found");

    }
    public async Task<Response<GetOrderDTO>> AddOrder(AddOrderDTO order)
    {
        var find = await _context.Orders.FirstOrDefaultAsync(o => o.Id == order.Id);
        if (find == null)
        {
            var mapped = _mapper.Map<Order>(order);
            await _context.Orders.AddAsync(mapped);
            await _context.SaveChangesAsync();
            return new Response<GetOrderDTO>(System.Net.HttpStatusCode.OK, "Order added successfully");
        }
        return new Response<GetOrderDTO>(System.Net.HttpStatusCode.BadGateway, "Order already exist");

    }

    public async Task<Response<GetOrderDTO>> UpdateOrder(AddOrderDTO order)
    {
        var find = _context.Orders.FirstOrDefaultAsync(o => o.Id == order.Id);
        if (find != null)
        {
            var mapped = _mapper.Map<Order>(find);
            await _context.Orders.AddAsync(mapped);
            await _context.SaveChangesAsync();
        }
        return new Response<GetOrderDTO>(System.Net.HttpStatusCode.NotFound, "Order Not Found");
    }
    public async Task<Response<GetOrderDTO>> DeleteOrder(int orderId)
    {
        var find = await _context.Orders.FirstOrDefaultAsync(o => o.Id == orderId);
        if (find != null)
        {
            _context.Orders.Remove(find);
            await _context.SaveChangesAsync();
            return new Response<GetOrderDTO>(System.Net.HttpStatusCode.OK, "Order deleted successfully");
        }
        return new Response<GetOrderDTO>(System.Net.HttpStatusCode.NotFound, "Order Not Found");
    }
}
