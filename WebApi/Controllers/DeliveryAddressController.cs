using Domain.DTOs.DeliveryAddressDTO;
using Domain.Filters.DeliveryAddressFilter;
using Domain.Responses;
using Infrastructure.Services.DeliveryAddressService;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DeliveryAddressController : BaseController
{
  private readonly IDeliveryAddressService _deliveryAddressService;
  public DeliveryAddressController(IDeliveryAddressService deliveryAddressService)
  {
    _deliveryAddressService = deliveryAddressService;
  }
  [HttpGet("get/deliveryAddress")]
  public async Task<IActionResult> GetDeliveryAddress([FromQuery] DeliveryAddressFilter filter)
  {
    if (ModelState.IsValid)
    {
      var result = await _deliveryAddressService.GetDeliveryAddress(filter);
      return StatusCode(result.StatusCode, result);
    }
    var response = new Response<List<GetDeliveryAddressDTO>>(System.Net.HttpStatusCode.BadRequest, ModelStateErrors());
    return StatusCode(response.StatusCode, response);
  }
  [HttpGet("get/deliveryAddressById")]
  public async Task<IActionResult> GetDeliveryAddressById(int deliveryAddressId)
  {
    if (ModelState.IsValid)
    {
      var result = await _deliveryAddressService.GetDeliveryAddressById(deliveryAddressId);
      return StatusCode(result.StatusCode, result);
    }
    var response = new Response<GetDeliveryAddressDTO>(System.Net.HttpStatusCode.BadRequest, ModelStateErrors());
    return StatusCode(response.StatusCode, response);
  }
  [HttpPost("post/deliveryAddress")]
  public async Task<IActionResult> AddDeliveryAddress(AddDeliveryAddressDTO deliveryAddress)
  {
    if (ModelState.IsValid)
    {
      var result = await _deliveryAddressService.AddDeliveryAddress(deliveryAddress);
      return StatusCode(result.StatusCode, result);
    }
    var response = new Response<GetDeliveryAddressDTO>(System.Net.HttpStatusCode.BadRequest, ModelStateErrors());
    return StatusCode(response.StatusCode, response);
  }
  [HttpPut("put/deliveryAddress")]
  public async Task<IActionResult> UpdateDeliveryAddress(AddDeliveryAddressDTO deliveryAddress)
  {
    if (ModelState.IsValid)
    {
      var result = await _deliveryAddressService.UpdateDeliveryAddress(deliveryAddress);
      return StatusCode(result.StatusCode, result);
    }
    var response = new Response<GetDeliveryAddressDTO>(System.Net.HttpStatusCode.BadRequest, ModelStateErrors());
    return StatusCode(response.StatusCode, response);
  }
  [HttpDelete("delete/deliveryaAddress")]
  public async Task<IActionResult> DeleteDeliveryAddress(int deliveryAddressId)
  {
    if (ModelState.IsValid)
    {
      var result = await _deliveryAddressService.DeleteDeliveryAddress(deliveryAddressId);
      return StatusCode(result.StatusCode, result);

    }
    var response = new Response<GetDeliveryAddressDTO>(System.Net.HttpStatusCode.BadRequest, ModelStateErrors());
    return StatusCode(response.StatusCode, response);
  }
}
