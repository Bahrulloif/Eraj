using Domain.DTOs.OrderDTO;
using Domain.Filters.OrderFilter;
using Domain.Responses;
using Infrastructure.Services.OrderService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrderController : BaseController
{
    private readonly IOrderService _orderservice;
    public OrderController(IOrderService orderService)
    {
        _orderservice = orderService;

    }
    [HttpGet("get/orders")]
    public async Task<ActionResult> GetOrder(OrderFilter filter)
    {
        if (ModelState.IsValid)
        {
            var result = await _orderservice.GetOrder(filter);
            return StatusCode(result.StatusCode, result);
        }
        var response = new Response<List<GetOrderDTO>>(System.Net.HttpStatusCode.BadRequest, ModelStateErrors());
        return StatusCode(response.StatusCode, response);
    }

    [HttpGet("get/ordersById")]
    public async Task<ActionResult> GetOrderById(int orderId)
    {
        if (ModelState.IsValid)
        {
            var result = await _orderservice.GetOrderById(orderId);
            return StatusCode(result.StatusCode, result);
        }
        var response = new Response<GetOrderDTO>(System.Net.HttpStatusCode.BadRequest, ModelStateErrors());
        return StatusCode(response.StatusCode, response);
    }

    [HttpPost("post/order")]
    public async Task<ActionResult> AddOrder(AddOrderDTO order)
    {
        if (ModelState.IsValid)
        {
            var result = await _orderservice.AddOrder(order);
            return StatusCode(result.StatusCode, result);
        }
        var response = new Response<GetOrderDTO>(System.Net.HttpStatusCode.BadRequest, ModelStateErrors());
        return StatusCode(response.StatusCode, response);
    }

    [HttpPut("put/order")]
    public async Task<ActionResult> UpdateOrder(AddOrderDTO order)
    {
        if (ModelState.IsValid)
        {
            var result = await _orderservice.UpdateOrder(order);
            return StatusCode(result.StatusCode, result);
        }
        var response = new Response<GetOrderDTO>(System.Net.HttpStatusCode.BadRequest, ModelStateErrors());
        return StatusCode(response.StatusCode, response);
    }

    [HttpDelete("delete/order")]
    public async Task<ActionResult> DeleteOrder(int orderId)
    {
        if (ModelState.IsValid)
        {
            var result = await _orderservice.DeleteOrder(orderId);
            return StatusCode(result.StatusCode, result);
        }
        var response = new Response<GetOrderDTO>(System.Net.HttpStatusCode.BadRequest, ModelStateErrors());
        return StatusCode(response.StatusCode, response);
    }

}
