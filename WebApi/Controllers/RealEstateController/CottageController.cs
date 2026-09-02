using Domain.DTOs.RealEstateDTOs.CottageDTOs;
using Domain.Filters.RealEstateFilters.CottageFilter;
using Domain.Responses;
using Infrastructure.Services.RealEstateService.CottageService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.RealEstateController;

[ApiController]
[Route("api/[controller]")]
public class CottageController : BaseController
{
    private readonly ICottageService _cottageService;
    public CottageController(ICottageService cottageService)
    {
        _cottageService = cottageService;
    }

    [HttpGet("get/cottage"), AllowAnonymous]
    public async Task<IActionResult> GetCottage([FromQuery] GetCottageFilter filter)
    {
        if (ModelState.IsValid)
        {
            var result = await _cottageService.GetCottage(filter);
            return StatusCode(result.StatusCode, result);
        }
        var response = new Response<List<GetCottageDTO>>(System.Net.HttpStatusCode.BadRequest, ModelStateErrors());
        return StatusCode(response.StatusCode, response);
    }

    [HttpGet("get/cottageById"), AllowAnonymous]
    public async Task<IActionResult> GetCottageById(int cottageId)
    {
        if (ModelState.IsValid)
        {
            var result = await _cottageService.GetCottageById(cottageId);
            return StatusCode(result.StatusCode, result);
        }
        var response = new Response<GetCottageDTO>(System.Net.HttpStatusCode.BadRequest, ModelStateErrors());
        return StatusCode(response.StatusCode, response);
    }

    [HttpPost("post/cottage")]
    [Authorize(Roles = "SuperAdmin, Admin, Businessman")]
    public async Task<IActionResult> AddCottage([FromForm] AddCottageDTO cottage)
    {
        if (ModelState.IsValid)
        {
            var result = await _cottageService.AddCottage(cottage);
            return StatusCode(result.StatusCode, result);
        }
        var response = new Response<GetCottageDTO>(System.Net.HttpStatusCode.BadRequest, ModelStateErrors());
        return StatusCode(response.StatusCode, response);
    }

    [HttpPut("put/cottage")]
    [Authorize(Roles = "SuperAdmin, Admin, Businessman")]
    public async Task<IActionResult> UpdateCottage([FromForm] AddCottageDTO cottage)
    {
        if (ModelState.IsValid)
        {
            var result = await _cottageService.UpdateCottage(cottage);
            return StatusCode(result.StatusCode, result);
        }
        var response = new Response<GetCottageDTO>(System.Net.HttpStatusCode.BadRequest, ModelStateErrors());
        return StatusCode(response.StatusCode, response);
    }

    [HttpDelete("delete/cottage")]
    [Authorize(Roles = "SuperAdmin, Admin, Businessman")]
    public async Task<IActionResult> DeleteCottage(int cottageId)
    {
        if (ModelState.IsValid)
        {
            var result = await _cottageService.DeleteCottage(cottageId);
            return StatusCode(result.StatusCode, result);
        }
        var response = new Response<GetCottageDTO>(System.Net.HttpStatusCode.BadRequest, ModelStateErrors());
        return StatusCode(response.StatusCode, response);
    }
}
