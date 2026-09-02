using Domain.DTOs.TransportDTOs.TruckDTOs;
using Domain.Filters.TransportFilter.TruckFilters;
using Domain.Responses;
using Infrastructure.Services.TransportService.TruckService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.TransportController;

[ApiController]
[Route("api/[controller]")]
public class TruckController : BaseController
{
    private readonly ITruckService _truckService;

    public TruckController(ITruckService truckService)
    {
        _truckService = truckService;
    }

    [HttpGet("get/GetTruck"), AllowAnonymous]
    public async Task<ActionResult> GetTruck([FromQuery] TruckFilter filter)
    {
        if (ModelState.IsValid)
        {
            var result = await _truckService.GetTruck(filter);
            return StatusCode(result.StatusCode, result);
        }
        var response = new Response<GetTruckDTO>(System.Net.HttpStatusCode.BadRequest, ModelStateErrors());
        return StatusCode(response.StatusCode, response);
    }

    [HttpGet("get/GetTruckById"), AllowAnonymous]
    public async Task<ActionResult> GetTruckById(int truckId)
    {
        if (ModelState.IsValid)
        {
            var result = await _truckService.GetTruckById(truckId);
            return StatusCode(result.StatusCode, result);
        }
        var response = new Response<GetTruckDTO>(System.Net.HttpStatusCode.BadRequest, ModelStateErrors());
        return StatusCode(response.StatusCode, response);
    }

    [HttpPost("post/AddTruck")]
    [Authorize(Roles = "SuperAdmin, Admin, Businessman")]
    public async Task<ActionResult> AddTruck([FromForm] AddTruckDTO truck)
    {
        if (ModelState.IsValid)
        {
            var result = await _truckService.AddTruck(truck, CurrentUserId!);
            return StatusCode(result.StatusCode, result);
        }
        var response = new Response<GetTruckDTO>(System.Net.HttpStatusCode.BadRequest, ModelStateErrors());
        return StatusCode(response.StatusCode, response);
    }

    [HttpPut("put/UpdateTruck")]
    [Authorize(Roles = "SuperAdmin, Admin, Businessman")]
    public async Task<ActionResult> UpdateTruck([FromForm] AddTruckDTO truck)
    {
        if (ModelState.IsValid)
        {
            var result = await _truckService.UpdateTruck(truck, CurrentUserId!, IsPrivilegedUser);
            return StatusCode(result.StatusCode, result);
        }
        var response = new Response<GetTruckDTO>(System.Net.HttpStatusCode.BadRequest, ModelStateErrors());
        return StatusCode(response.StatusCode, response);
    }
    [HttpDelete("delete/DeleteTruck")]
    [Authorize(Roles = "SuperAdmin, Admin, Businessman")]
    public async Task<ActionResult> DeleteTruck(int truckId)
    {
        if (ModelState.IsValid)
        {
            var result = await _truckService.DeleteTruck(truckId, CurrentUserId!, IsPrivilegedUser);
            return StatusCode(result.StatusCode, result);
        }
        var response = new Response<GetTruckDTO>(System.Net.HttpStatusCode.BadRequest, ModelStateErrors());
        return StatusCode(response.StatusCode, response);
    }
}
