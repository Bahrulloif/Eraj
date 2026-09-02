using Domain.DTOs.CartDTO;
using Domain.Filters.CartFilter;
using Domain.Responses;

namespace Infrastructure.Services.CartService;

public interface ICartService
{
    Task<Response<List<GetCartDTO>>> GetCart(CartFilter filter, string currentUserId, bool isPrivileged);
    Task<Response<GetCartDTO>> GetCartById(int cartId, string currentUserId, bool isPrivileged);
    Task<Response<string>> AddCart(AddCartDTO cart, string currentUserId);
    Task<Response<string>> UpdateCart(AddCartDTO cart, string currentUserId, bool isPrivileged);
    Task<Response<GetCartDTO>> DeleteCart(int cartId, string currentUserId, bool isPrivileged);
}
