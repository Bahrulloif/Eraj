using Domain.DTOs.TransportDTOs.CarsDTOs;
using Domain.Filters.TransportFilters.CarsFilter;
using Domain.Responses;
using Infrastructure.Services.TransportService.CarService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.TransportController;

[ApiController]
[Route("api/[controller]")]
public class CarController : BaseController
{
    private readonly ICarService _carService;
    public CarController(ICarService carService)
    {
        _carService = carService;
    }
    [HttpGet("get/car"), AllowAnonymous]
    public async Task<IActionResult> GetCar([FromQuery] GetCarFilter filter)
    {
        if (ModelState.IsValid)
        {
            var result = await _carService.GetCar(filter);
            return StatusCode(result.StatusCode, result);
        }
        var response = new Response<List<GetCarDTO>>(System.Net.HttpStatusCode.BadRequest, ModelStateErrors());
        return StatusCode(response.StatusCode, response);
    }

    [HttpGet("get/carById"), AllowAnonymous]
    public async Task<ActionResult> GetCarById(int carId)
    {
        if (ModelState.IsValid)
        {
            var result = await _carService.GetCarById(carId);
            return StatusCode(result.StatusCode, result);
        }
        var response = new Response<GetCarDTO>(System.Net.HttpStatusCode.BadRequest, ModelStateErrors());
        return StatusCode(response.StatusCode, response);
    }

    [HttpPost("post/addCar")]
    public async Task<ActionResult> AddCar([FromForm] AddCarDTO car)
    {
        if (ModelState.IsValid)
        {
            var result = await _carService.AddCar(car);
            return StatusCode(result.StatusCode, result);
        }
        var response = new Response<GetCarDTO>(System.Net.HttpStatusCode.BadGateway, ModelStateErrors());
        return StatusCode(response.StatusCode, response);
    }

    [HttpPost("post/updateCar")]
    public async Task<ActionResult> UpdateCar([FromForm] AddCarDTO car)
    {
        if (ModelState.IsValid)
        {
            var result = await _carService.UpdateCar(car);
            return StatusCode(result.StatusCode, result);
        }
        var response = new Response<GetCarDTO>(System.Net.HttpStatusCode.BadRequest, ModelStateErrors());
        return StatusCode(response.StatusCode, response);
    }

    [HttpDelete("delete/deleteCar")]
    public async Task<ActionResult> DeleteCar(int carId)
    {
        if (ModelState.IsValid)
        {
            var result = await _carService.DeleteCar(carId);
            return StatusCode(result.StatusCode, result);
        }
        var response = new Response<GetCarDTO>(System.Net.HttpStatusCode.BadGateway, ModelStateErrors());
        return StatusCode(response.StatusCode, response);
    }
}
