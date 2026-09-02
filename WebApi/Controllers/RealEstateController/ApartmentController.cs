using Domain.DTOs.RealEstateDTOs.ApartmentDTOs;
using Domain.Filters.RealEstateFilters.ApartmentFilter;
using Domain.Responses;
using Infrastructure.Services.RealEstateService.ApartmentService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.RealEstateController;

[ApiController]
[Route("api/[controller]")]
public class ApartmentController : BaseController
{
    private readonly IApartmentService _apartmentService;
    public ApartmentController(IApartmentService apartmentService)
    {
        _apartmentService = apartmentService;
    }

    [HttpGet("get/apartment"), AllowAnonymous]
    public async Task<IActionResult> GetApartment([FromQuery] GetApartmentFilter filter)
    {
        if (ModelState.IsValid)
        {
            var result = await _apartmentService.GetApartment(filter);
            return StatusCode(result.StatusCode, result);
        }
        var response = new Response<List<GetApartmentDTO>>(System.Net.HttpStatusCode.BadRequest, ModelStateErrors());
        return StatusCode(response.StatusCode, response);
    }

    [HttpGet("get/apartmentById"), AllowAnonymous]
    public async Task<IActionResult> GetApartmentById(int apartmentId)
    {
        if (ModelState.IsValid)
        {
            var result = await _apartmentService.GetApartmentById(apartmentId);
            return StatusCode(result.StatusCode, result);
        }
        var response = new Response<GetApartmentDTO>(System.Net.HttpStatusCode.BadRequest, ModelStateErrors());
        return StatusCode(response.StatusCode, response);
    }

    [HttpPost("post/apartment")]
    [Authorize(Roles = "SuperAdmin, Admin, Businessman")]
    public async Task<IActionResult> AddApartment([FromForm] AddApartmentDTO apartment)
    {
        if (ModelState.IsValid)
        {
            var result = await _apartmentService.AddApartment(apartment, CurrentUserId!);
            return StatusCode(result.StatusCode, result);
        }
        var response = new Response<GetApartmentDTO>(System.Net.HttpStatusCode.BadRequest, ModelStateErrors());
        return StatusCode(response.StatusCode, response);
    }

    [HttpPut("put/apartment")]
    [Authorize(Roles = "SuperAdmin, Admin, Businessman")]
    public async Task<IActionResult> UpdateApartment([FromForm] AddApartmentDTO apartment)
    {
        if (ModelState.IsValid)
        {
            var result = await _apartmentService.UpdateApartment(apartment, CurrentUserId!, IsPrivilegedUser);
            return StatusCode(result.StatusCode, result);
        }
        var response = new Response<GetApartmentDTO>(System.Net.HttpStatusCode.BadRequest, ModelStateErrors());
        return StatusCode(response.StatusCode, response);
    }

    [HttpDelete("delete/apartment")]
    [Authorize(Roles = "SuperAdmin, Admin, Businessman")]
    public async Task<IActionResult> DeleteApartment(int apartmentId)
    {
        if (ModelState.IsValid)
        {
            var result = await _apartmentService.DeleteApartment(apartmentId, CurrentUserId!, IsPrivilegedUser);
            return StatusCode(result.StatusCode, result);
        }
        var response = new Response<GetApartmentDTO>(System.Net.HttpStatusCode.BadRequest, ModelStateErrors());
        return StatusCode(response.StatusCode, response);
    }
}
