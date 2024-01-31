using Domain.DTOs.CartDTO;
using Domain.Filters.CartFilter;
using Domain.Responses;
using Infrastructure.Services.CartService;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CartController : BaseController
{
    private readonly ICartService _cartService;
    public CartController(ICartService cartService)
    {
        _cartService = cartService;
    }
    [HttpGet("get/cart")]
    public async Task<IActionResult> GetCart([FromQuery] CartFilter filter)
    {
        if (ModelState.IsValid)
        {
            var result = await _cartService.GetCart(filter);
            return StatusCode(result.StatusCode, result);
        }
        var response = new Response<List<GetCartDTO>>(System.Net.HttpStatusCode.BadRequest, ModelStateErrors());
        return StatusCode(response.StatusCode, response);
    }
    [HttpGet("get/cartById")]
    public async Task<IActionResult> GetCartById(int cartId)
    {
        if (ModelState.IsValid)
        {
            var result = await _cartService.GetCartById(cartId);
            return StatusCode(result.StatusCode, result);
        }
        var response = new Response<GetCartDTO>(System.Net.HttpStatusCode.BadRequest, ModelStateErrors());
        return StatusCode(response.StatusCode, response);
    }
    [HttpPost("post/cart")]
    public async Task<IActionResult> AddCart(AddCartDTO cart)
    {
        if (ModelState.IsValid)
        {
            var result = await _cartService.AddCart(cart);
            return StatusCode(result.StatusCode, result);
        }
        var response = new Response<GetCartDTO>(System.Net.HttpStatusCode.BadRequest, ModelStateErrors());
        return StatusCode(response.StatusCode, response);
    }
    [HttpPut("put/cart")]
    public async Task<IActionResult> UpdateCart(AddCartDTO cart)
    {
        if (ModelState.IsValid)
        {
            var result = await _cartService.UpdateCart(cart);
            return StatusCode(result.StatusCode, result);
        }
        var response = new Response<GetCartDTO>(System.Net.HttpStatusCode.BadRequest, ModelStateErrors());
        return StatusCode(response.StatusCode, response);
    }
    [HttpDelete("delete/cart")]
    public async Task<IActionResult> DeleteCart(int cartId)
    {
        if (ModelState.IsValid)
        {
            var result = await _cartService.DeleteCart(cartId);
            return StatusCode(result.StatusCode, result);
        }
        var response = new Response<GetCartDTO>(System.Net.HttpStatusCode.BadRequest, ModelStateErrors());
        return StatusCode(response.StatusCode, response);
    }
}
