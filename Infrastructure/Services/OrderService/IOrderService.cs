using Domain.DTOs.OrderDTO;
using Domain.Filters.OrderFilter;
using Domain.Responses;

namespace Infrastructure.Services.OrderService;

public interface IOrderService
{
    Task<Response<List<GetOrderDTO>>> GetOrder(OrderFilter filter, string currentUserId, bool isPrivileged);
    Task<Response<GetOrderDTO>> GetOrderById(int orderId, string currentUserId, bool isPrivileged);
    Task<Response<GetOrderDTO>> AddOrder(AddOrderDTO order, string currentUserId);
    Task<Response<GetOrderDTO>> UpdateOrder(AddOrderDTO order, string currentUserId, bool isPrivileged);
    Task<Response<GetOrderDTO>> DeleteOrder(int orderId, string currentUserId, bool isPrivileged);
}
