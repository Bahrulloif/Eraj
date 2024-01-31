using Domain.DTOs.KomTechDTOs.SmartPhoneDTOs;
using Domain.Filters.KompTechFilters.SmartPhoneFilters;
using Domain.Responses;
using Infrastructure.Services.KompTechService.SmartPhoneService;
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

    [HttpGet("get/smartphone")]
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

    [HttpGet("get/smartphoneById")]
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
    public async Task<IActionResult> AddSmartPhone(AddSmartPhoneDTO smartPhone)
    {
        if (ModelState.IsValid)
        {
            var result = await _smartPhoneService.AddSmartPhone(smartPhone);
            return StatusCode(result.StatusCode, result);
        }
        var response = new Response<GetSmartPhoneDTO>(System.Net.HttpStatusCode.BadRequest, ModelStateErrors());
        return StatusCode(response.StatusCode, response);
    }

    [HttpPut("put/smartphone")]
    public async Task<IActionResult> UpdateSmartPhone(AddSmartPhoneDTO smartPhone)
    {
        if (ModelState.IsValid)
        {
            var result = await _smartPhoneService.UpdateSmartPhone(smartPhone);
            return StatusCode(result.StatusCode, result);
        }
        var response = new Response<GetSmartPhoneDTO>(System.Net.HttpStatusCode.BadRequest, ModelStateErrors());
        return StatusCode(response.StatusCode, response);
    }

    [HttpDelete("delete/smartphone")]
    public async Task<ActionResult> DeleteSmartPhone(int smartPhoneId)
    {
        if (ModelState.IsValid)
        {
            var result = await _smartPhoneService.DeleteSmartPhone(smartPhoneId);
            return StatusCode(result.StatusCode, result);
        }
        var response = new Response<GetSmartPhoneDTO>(System.Net.HttpStatusCode.NotFound, ModelStateErrors());
        return StatusCode(response.StatusCode, response);
    }
}
