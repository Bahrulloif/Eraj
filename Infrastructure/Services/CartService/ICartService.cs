using Domain.DTOs.CartDTO;
using Domain.Filters.CartFilter;
using Domain.Responses;

namespace Infrastructure.Services.CartService;

public interface ICartService
{
    Task<Response<List<GetCartDTO>>> GetCart(CartFilter filter);
    Task<Response<GetCartDTO>> GetCartById(int cartId);
    Task<Response<string>> AddCart(AddCartDTO cart);
    Task<Response<string>> UpdateCart(AddCartDTO cart);
    Task<Response<GetCartDTO>> DeleteCart(int cartId);
}
