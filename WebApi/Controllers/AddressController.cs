using Domain.DTOs.AddressDTO;
using Domain.Filters.AddressFilter;
using Domain.Responses;
using Infrastructure.Services.AddressService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AddressController : BaseController
{
    private readonly IAddressService _addressService;
    public AddressController(IAddressService addressService)
    {
        _addressService = addressService;
    }
    [HttpGet("get/address")]
    [AllowAnonymous]
    public async Task<IActionResult> GetAddress([FromQuery]AddressFilter filter)
    {
        if (ModelState.IsValid)
        {
            var result = await _addressService.GetAddress(filter);
            return StatusCode(result.StatusCode, result);
        }
        var response = new Response<List<GetAddressDTO>>(System.Net.HttpStatusCode.BadRequest, ModelStateErrors());
        return StatusCode(response.StatusCode, response);
    }
    [HttpGet("get/addressById")]
    public async Task<IActionResult> GetAddressById(int addressId)
    {
        if (ModelState.IsValid)
        {
            var result = await _addressService.GetAddressById(addressId);
            return StatusCode(result.StatusCode, result);
        }
        var response = new Response<GetAddressDTO>(System.Net.HttpStatusCode.BadRequest, ModelStateErrors());
        return StatusCode(response.StatusCode, response);
    }
    [HttpPost("post/address")]
    public async Task<IActionResult> AddAddress(AddAddressDTO address)
    {
        if (ModelState.IsValid)
        {
            var result = await _addressService.AddAddress(address);
            return StatusCode(result.StatusCode, result);
        }
        var response = new Response<GetAddressDTO>(System.Net.HttpStatusCode.BadRequest, ModelStateErrors());
        return StatusCode(response.StatusCode, response);
    }
    [HttpPut("put/address")]
    public async Task<IActionResult> UpdateAddress(UpdateAddressDTO address)
    {
        if (ModelState.IsValid)
        {
            var result = await _addressService.UpdateAddress(address);
            return StatusCode(result.StatusCode, result);
        }
        var response = new Response<GetAddressDTO>(System.Net.HttpStatusCode.BadRequest, ModelStateErrors());
        return StatusCode(response.StatusCode, response);   
    }
    [HttpDelete("delete/address")]
    public async Task<IActionResult> DeleteAddress(int addressId)
    {
        if (ModelState.IsValid)
        {
            var result = await _addressService.DeleteAddress(addressId);
            return StatusCode(result.StatusCode, result);
        }
        var response = new Response<GetAddressDTO>(System.Net.HttpStatusCode.BadRequest, ModelStateErrors());
        return StatusCode(response.StatusCode, response);
    }
}
