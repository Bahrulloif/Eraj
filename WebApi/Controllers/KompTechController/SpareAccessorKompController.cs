using Domain.DTOs.KomTechDTOs.SpareAccessorKompDTOs;
using Domain.Filters.KompTechFilters.SpareAccessorKompFilter;
using Domain.Responses;
using Infrastructure.Services.KompTechService.SpareAccessorKompService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.KompTechController;

[ApiController]
[Route("api/[controller]")]
public class SpareAccessorKompController : BaseController
{
    private readonly ISpareAccessorKompService _spareAccessorKompService;
    public SpareAccessorKompController(ISpareAccessorKompService spareAccessorKompService)
    {
        _spareAccessorKompService = spareAccessorKompService;
    }

    [HttpGet("get/spareAccessorKomp"), AllowAnonymous]
    public async Task<IActionResult> GetSpareAccessorKomp([FromQuery] GetSpareAccessorKompFilter filter)
    {
        if (ModelState.IsValid)
        {
            var result = await _spareAccessorKompService.GetSpareAccessorKomp(filter);
            return StatusCode(result.StatusCode, result);
        }
        var response = new Response<List<GetSpareAccessorKompDTO>>(System.Net.HttpStatusCode.BadRequest, ModelStateErrors());
        return StatusCode(response.StatusCode, response);
    }

    [HttpGet("get/spareAccessorKompById"), AllowAnonymous]
    public async Task<IActionResult> GetSpareAccessorKompById(int spareAccessorKompId)
    {
        if (ModelState.IsValid)
        {
            var result = await _spareAccessorKompService.GetSpareAccessorKompById(spareAccessorKompId);
            return StatusCode(result.StatusCode, result);
        }
        var response = new Response<GetSpareAccessorKompDTO>(System.Net.HttpStatusCode.BadRequest, ModelStateErrors());
        return StatusCode(response.StatusCode, response);
    }

    [HttpPost("post/spareAccessorKomp")]
    public async Task<IActionResult> AddSpareAccessorKomp([FromForm] AddSpareAccessorKompDTO spareAccessorKomp)
    {
        if (ModelState.IsValid)
        {
            var result = await _spareAccessorKompService.AddSpareAccessorKomp(spareAccessorKomp);
            return StatusCode(result.StatusCode, result);
        }
        var response = new Response<string>(System.Net.HttpStatusCode.BadRequest, ModelStateErrors());
        return StatusCode(response.StatusCode, response);
    }

    [HttpPut("put/spareAccessorKomp")]
    public async Task<IActionResult> UpdateSpareAccessorKomp([FromForm] AddSpareAccessorKompDTO spareAccessorKomp)
    {
        if (ModelState.IsValid)
        {
            var result = await _spareAccessorKompService.UpdateSpareAccessorKomp(spareAccessorKomp);
            return StatusCode(result.StatusCode, result);
        }
        var response = new Response<string>(System.Net.HttpStatusCode.BadRequest, ModelStateErrors());
        return StatusCode(response.StatusCode, response);
    }

    [HttpDelete("delete/spareAccessorKomp")]
    public async Task<IActionResult> DeleteSpareAccessorKomp(int spareAccessorKompId)
    {
        if (ModelState.IsValid)
        {
            var result = await _spareAccessorKompService.DeleteSpareAccessorKomp(spareAccessorKompId);
            return StatusCode(result.StatusCode, result);
        }
        var response = new Response<string>(System.Net.HttpStatusCode.BadRequest, ModelStateErrors());
        return StatusCode(response.StatusCode, response);
    }
}
