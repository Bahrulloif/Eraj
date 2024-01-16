using Domain.DTOs.OrderDTO;
using Domain.Filters.OrderFilter;
using Domain.Responses;

namespace Infrastructure.Services.OrderService;

public interface IOrderService
{
    Task<Response<List<GetOrderDTO>>> GetOrder(OrderFilter filter);
    Task<Response<GetOrderDTO>> GetOrderById(int orderId);
    Task<Response<GetOrderDTO>> AddOrder(AddOrderDTO order);
    Task<Response<GetOrderDTO>> UpdateOrder(AddOrderDTO order);
    Task<Response<GetOrderDTO>> DeleteOrder(int orderId);
}
