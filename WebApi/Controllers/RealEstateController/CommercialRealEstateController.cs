using Domain.DTOs.RealEstateDTOs.CommercialRealEstateDTOs;
using Domain.Filters.RealEstateFilters.CommercialRealEstateFilter;
using Domain.Responses;
using Infrastructure.Services.RealEstateService.CommercialRealEstateService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.RealEstateController;

[ApiController]
[Route("api/[controller]")]
public class CommercialRealEstateController : BaseController
{
    private readonly ICommercialRealEstateService _commercialRealEstateService;
    public CommercialRealEstateController(ICommercialRealEstateService commercialRealEstateService)
    {
        _commercialRealEstateService = commercialRealEstateService;
    }

    [HttpGet("get/commercialRealEstate"), AllowAnonymous]
    public async Task<IActionResult> GetCommercialRealEstate([FromQuery] GetCommercialRealEstateFilter filter)
    {
        if (ModelState.IsValid)
        {
            var result = await _commercialRealEstateService.GetCommercialRealEstate(filter);
            return StatusCode(result.StatusCode, result);
        }
        var response = new Response<List<GetCommercialRealEstateDTO>>(System.Net.HttpStatusCode.BadRequest, ModelStateErrors());
        return StatusCode(response.StatusCode, response);
    }

    [HttpGet("get/commercialRealEstateById"), AllowAnonymous]
    public async Task<IActionResult> GetCommercialRealEstateById(int commercialRealEstateId)
    {
        if (ModelState.IsValid)
        {
            var result = await _commercialRealEstateService.GetCommercialRealEstateById(commercialRealEstateId);
            return StatusCode(result.StatusCode, result);
        }
        var response = new Response<GetCommercialRealEstateDTO>(System.Net.HttpStatusCode.BadRequest, ModelStateErrors());
        return StatusCode(response.StatusCode, response);
    }

    [HttpPost("post/commercialRealEstate")]
    [Authorize(Roles = "SuperAdmin, Admin, Businessman")]
    public async Task<IActionResult> AddCommercialRealEstate([FromForm] AddCommercialRealEstateDTO commercialRealEstate)
    {
        if (ModelState.IsValid)
        {
            var result = await _commercialRealEstateService.AddCommercialRealEstate(commercialRealEstate);
            return StatusCode(result.StatusCode, result);
        }
        var response = new Response<GetCommercialRealEstateDTO>(System.Net.HttpStatusCode.BadRequest, ModelStateErrors());
        return StatusCode(response.StatusCode, response);
    }

    [HttpPut("put/commercialRealEstate")]
    [Authorize(Roles = "SuperAdmin, Admin, Businessman")]
    public async Task<IActionResult> UpdateCommercialRealEstate([FromForm] AddCommercialRealEstateDTO commercialRealEstate)
    {
        if (ModelState.IsValid)
        {
            var result = await _commercialRealEstateService.UpdateCommercialRealEstate(commercialRealEstate);
            return StatusCode(result.StatusCode, result);
        }
        var response = new Response<GetCommercialRealEstateDTO>(System.Net.HttpStatusCode.BadRequest, ModelStateErrors());
        return StatusCode(response.StatusCode, response);
    }

    [HttpDelete("delete/commercialRealEstate")]
    [Authorize(Roles = "SuperAdmin, Admin, Businessman")]
    public async Task<IActionResult> DeleteCommercialRealEstate(int commercialRealEstateId)
    {
        if (ModelState.IsValid)
        {
            var result = await _commercialRealEstateService.DeleteCommercialRealEstate(commercialRealEstateId);
            return StatusCode(result.StatusCode, result);
        }
        var response = new Response<GetCommercialRealEstateDTO>(System.Net.HttpStatusCode.BadRequest, ModelStateErrors());
        return StatusCode(response.StatusCode, response);
    }
}
