using Domain.DTOs.TransportDTOs.SpareAccessorTranspDTOs;
using Domain.Filters.TransportFilter.SpareAccessorTranspFilters;
using Domain.Responses;
using Infrastructure.Services.TransportService.SpareAccessorTranspService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.TransportController;

[ApiController]
[Route("api/[controller]")]
public class SpareAccessorTranspController : BaseController
{
    private readonly ISpareAccessorTranspService _spareAccessorTranspService;
    public SpareAccessorTranspController(ISpareAccessorTranspService spareAccessorTranspService)
    {
        _spareAccessorTranspService = spareAccessorTranspService;
    }

    [HttpGet("get/spareAccessorTransp"), AllowAnonymous]
    public async Task<IActionResult> GetSpareAccessorTransp([FromQuery] GetSpareAccessorTranspFilter filter)
    {
        if (ModelState.IsValid)
        {
            var result = await _spareAccessorTranspService.GetSpareAccessorTransp(filter);
            return StatusCode(result.StatusCode, result);
        }
        var response = new Response<List<GetSpareAccessorTranspDTO>>(System.Net.HttpStatusCode.BadRequest, ModelStateErrors());
        return StatusCode(response.StatusCode, response);
    }

    [HttpGet("get/spareAccessorTranspById"), AllowAnonymous]
    public async Task<IActionResult> GetSpareAccessorTranspById(int spareAccessorTranspId)
    {
        if (ModelState.IsValid)
        {
            var result = await _spareAccessorTranspService.GetSpareAccessorTranspById(spareAccessorTranspId);
            return StatusCode(result.StatusCode, result);
        }
        var response = new Response<GetSpareAccessorTranspDTO>(System.Net.HttpStatusCode.BadRequest, ModelStateErrors());
        return StatusCode(response.StatusCode, response);
    }

    [HttpPost("post/spareAccessorTransp")]
    [Authorize(Roles = "SuperAdmin, Admin, Businessman")]
    public async Task<IActionResult> AddSpareAccessorTransp([FromForm] AddSpareAccessorTranspDTO spareAccessorTransp)
    {
        if (ModelState.IsValid)
        {
            var result = await _spareAccessorTranspService.AddSpareAccessorTransp(spareAccessorTransp);
            return StatusCode(result.StatusCode, result);
        }
        var response = new Response<string>(System.Net.HttpStatusCode.BadRequest, ModelStateErrors());
        return StatusCode(response.StatusCode, response);
    }

    [HttpPut("put/spareAccessorTransp")]
    [Authorize(Roles = "SuperAdmin, Admin, Businessman")]
    public async Task<IActionResult> UpdateSpareAccessorTransp([FromForm] AddSpareAccessorTranspDTO spareAccessorTransp)
    {
        if (ModelState.IsValid)
        {
            var result = await _spareAccessorTranspService.UpdateSpareAccessorTransp(spareAccessorTransp);
            return StatusCode(result.StatusCode, result);
        }
        var response = new Response<string>(System.Net.HttpStatusCode.BadRequest, ModelStateErrors());
        return StatusCode(response.StatusCode, response);
    }

    [HttpDelete("delete/spareAccessorTransp")]
    [Authorize(Roles = "SuperAdmin, Admin, Businessman")]
    public async Task<IActionResult> DeleteSpareAccessorTransp(int spareAccessorTranspId)
    {
        if (ModelState.IsValid)
        {
            var result = await _spareAccessorTranspService.DeleteSpareAccessorTransp(spareAccessorTranspId);
            return StatusCode(result.StatusCode, result);
        }
        var response = new Response<string>(System.Net.HttpStatusCode.BadRequest, ModelStateErrors());
        return StatusCode(response.StatusCode, response);
    }
}
