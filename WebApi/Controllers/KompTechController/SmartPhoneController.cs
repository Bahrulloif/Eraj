using Domain.DTOs.KomTechDTOs.SmartPhoneDTOs;
using Domain.Filters.KompTechFilters.SmartPhoneFilters;
using Domain.Responses;
using Infrastructure.Services.KompTechService.SmartPhoneService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace WebApi.Controllers.KompTechController;

[ApiController]
[Route("api/[controller]")]
public class SmartPhoneController : BaseController
{
    private readonly ISmartPhoneService _smartPhoneService;
    public SmartPhoneController(ISmartPhoneService smartPhoneService)
    {
        _smartPhoneService = smartPhoneService;
    }

    [HttpGet("get/smartphone"), AllowAnonymous]
    public async Task<IActionResult> GetSmartPhone([FromQuery] GetSmartPhoneFilter filter)
    {
        if (ModelState.IsValid)
        {
            var result = await _smartPhoneService.GetSmartPhone(filter);
            return StatusCode(result.StatusCode, result);
        }
        var response = new Response<List<GetSmartPhoneDTO>>(System.Net.HttpStatusCode.BadRequest, ModelStateErrors());
        return StatusCode(response.StatusCode, response);
    }

    [HttpGet("get/smartphoneById"), AllowAnonymous]
    public async Task<IActionResult> GetSmartPhoneById(int smartPhoneId)
    {
        if (ModelState.IsValid)
        {
            var result = await _smartPhoneService.GetSmartPhoneById(smartPhoneId);
            return StatusCode(result.StatusCode, result);
        }
        var response = new Response<GetSmartPhoneDTO>(System.Net.HttpStatusCode.BadRequest, ModelStateErrors());
        return StatusCode(response.StatusCode, response);
    }

    [HttpPost("post/smartphone")]
    [Authorize(Roles = "SuperAdmin, Admin, Businessman")]
    public async Task<IActionResult> AddSmartPhone([FromForm] AddSmartPhoneDTO smartPhone)
    {
        if (ModelState.IsValid)
        {
            var result = await _smartPhoneService.AddSmartPhone(smartPhone, CurrentUserId!);
            return StatusCode(result.StatusCode, result);
        }
        var response = new Response<GetSmartPhoneDTO>(System.Net.HttpStatusCode.BadRequest, ModelStateErrors());
        return StatusCode(response.StatusCode, response);
    }

    [HttpPut("put/smartphone")]
    [Authorize(Roles = "SuperAdmin, Admin, Businessman")]
    public async Task<IActionResult> UpdateSmartPhone([FromForm]AddSmartPhoneDTO smartPhone)
    {
        if (ModelState.IsValid)
        {
            var result = await _smartPhoneService.UpdateSmartPhone(smartPhone, CurrentUserId!, IsPrivilegedUser);
            return StatusCode(result.StatusCode, result);
        }
        var response = new Response<GetSmartPhoneDTO>(System.Net.HttpStatusCode.BadRequest, ModelStateErrors());
        return StatusCode(response.StatusCode, response);
    }

    [HttpDelete("delete/smartphone")]
    [Authorize(Roles = "SuperAdmin, Admin, Businessman")]
    public async Task<ActionResult> DeleteSmartPhone(int smartPhoneId)
    {
        if (ModelState.IsValid)
        {
            var result = await _smartPhoneService.DeleteSmartPhone(smartPhoneId, CurrentUserId!, IsPrivilegedUser);
            return StatusCode(result.StatusCode, result);
        }
        var response = new Response<GetSmartPhoneDTO>(System.Net.HttpStatusCode.NotFound, ModelStateErrors());
        return StatusCode(response.StatusCode, response);
    }
}
