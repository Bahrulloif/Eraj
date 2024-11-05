using Domain.DTOs.TransportDTOs.MotorbikeDTOs;
using Domain.Filters.TransportFilters.GetMotorbikeFilter;
using Domain.Responses;
using Infrastructure.Services.TransportService.MotorbikeService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.TransportController;

[ApiController]
[Route("api/[controller]")]
public class MotorbikeController : BaseController
{
    private readonly IMotorbikeService _motorbikeService;
    public MotorbikeController(IMotorbikeService motorbikeService)
    {
        _motorbikeService = motorbikeService;
    }
    [HttpGet("get/getMotorbike"), AllowAnonymous]
    public async Task<ActionResult> GetMotorbike([FromQuery] GetMotorbikeFilter filter)
    {
        if (ModelState.IsValid)
        {
            var result = await _motorbikeService.GetMotorbike(filter);
            return StatusCode(result.StatusCode, result);
        }
        var response = new Response<GetMotorbikeDTO>(System.Net.HttpStatusCode.BadGateway, ModelStateErrors());
        return StatusCode(response.StatusCode, response);
    }
    [HttpGet("get/getMotorbikeById"), AllowAnonymous]
    public async Task<ActionResult> GetMotorbikeById(int motorbikeId)
    {
        if (ModelState.IsValid)
        {
            var result = await _motorbikeService.GetMotorbikeById(motorbikeId);
            return StatusCode(result.StatusCode, result);
        }
        var response = new Response<GetMotorbikeDTO>(System.Net.HttpStatusCode.BadRequest, ModelStateErrors());
        return StatusCode(response.StatusCode, response);

    }
    [HttpPost("post/addMotorbike")]
    public async Task<ActionResult> AddMotorbike([FromForm] AddMotorbikeDTO motorbike)
    {
        if (ModelState.IsValid)
        {
            var result = await _motorbikeService.AddMotorbike(motorbike);
            return StatusCode(result.StatusCode, result);
        }
        var response = new Response<GetMotorbikeDTO>(System.Net.HttpStatusCode.BadRequest, ModelStateErrors());
        return StatusCode(response.StatusCode, response);
    }

    [HttpPut("put/updateMotorbike")]
    public async Task<ActionResult> UpdateMotorbike([FromForm] AddMotorbikeDTO motorbike)
    {
        if (ModelState.IsValid)
        {
            var result = await _motorbikeService.UpdateMotorbike(motorbike);
            return StatusCode(result.StatusCode, result);
        }
        var response = new Response<GetMotorbikeDTO>(System.Net.HttpStatusCode.BadGateway, ModelStateErrors());
        return StatusCode(response.StatusCode, response);
    }

    [HttpDelete("delete/deleteMotorbike")]
    public async Task<ActionResult> DeleteMotorbike(int motorbikeId)
    {
        if (ModelState.IsValid)
        {
            var result = await _motorbikeService.DeleteMotorbike(motorbikeId);
            return StatusCode(result.StatusCode, result);
        }
        var response = new Response<GetMotorbikeDTO>(System.Net.HttpStatusCode.BadRequest, ModelStateErrors());
        return StatusCode(response.StatusCode, response);
    }
}
