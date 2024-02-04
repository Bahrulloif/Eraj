using Domain.DTOs.KomTechDTOs.TabletDTOs;
using Domain.Filters.KompTechFilters.TabletFilter;
using Domain.Responses;
using Infrastructure.Services.KompTechService.TabletService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace WebApi.Controllers.KompTechController;

[ApiController]
[Route("api/[controller]")]
public class TabletController : BaseController
{
    private readonly ITabletService _tabletService;
    public TabletController(ITabletService tabletService)
    {
        _tabletService = tabletService;
    }

    [HttpGet("get/tablet")]
    public async Task<IActionResult> GetTablet([FromQuery] GetTabletFilter filter)
    {
        if (ModelState.IsValid)
        {
            var result = await _tabletService.GetTablet(filter);
            return StatusCode(result.StatusCode, result);
        }
        var response = new Response<List<GetTabletDTO>>(System.Net.HttpStatusCode.BadRequest, ModelStateErrors());
        return StatusCode(response.StatusCode, response);
    }


    [HttpGet("get/tabletById")]
    public async Task<IActionResult> GetTabletById(int tabletId)
    {
        if (ModelState.IsValid)
        {
            var result = await _tabletService.GetTabletById(tabletId);
            return StatusCode(result.StatusCode, result);
        }
        var response = new Response<GetTabletDTO>(System.Net.HttpStatusCode.BadRequest, ModelStateErrors());
        return StatusCode(response.StatusCode, response);
    }

    [HttpPost("post/tablet")]
    public async Task<IActionResult> AddTablet([FromForm] AddTabletDTO tablet)
    {
        if (ModelState.IsValid)
        {
            var result = await _tabletService.AddTablet(tablet);
            return StatusCode(result.StatusCode, result);
        }
        var response = new Response<GetTabletDTO>(System.Net.HttpStatusCode.BadRequest, ModelStateErrors());
        return StatusCode(response.StatusCode, response);
    }

    [HttpPut("put/tablet")]
    public async Task<IActionResult> UpdateTablet([FromForm] AddTabletDTO tablet)
    {
        if (ModelState.IsValid)
        {
            var result = await _tabletService.UpdateTablet(tablet);
            return StatusCode(result.StatusCode, result);
        }
        var response = new Response<GetTabletDTO>(System.Net.HttpStatusCode.BadRequest, ModelStateErrors());
        return StatusCode(response.StatusCode, response);
    }

    [HttpDelete("delete/tablet")]
    public async Task<IActionResult> DeleteTablet(int tabletId)
    {
        if (ModelState.IsValid)
        {
            var result = await _tabletService.DeleteTablet(tabletId);
            return StatusCode(result.StatusCode, result);
        }
        var response = new Response<GetTabletDTO>(System.Net.HttpStatusCode.BadRequest, ModelStateErrors());
        return StatusCode(response.StatusCode, response);
    }
}
