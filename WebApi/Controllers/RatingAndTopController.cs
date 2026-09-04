using Domain.DTOs.RatingAndTopDTO;
using Domain.Filters.RatingAndTopFilter;
using Domain.Responses;
using Infrastructure.Services;
using Infrastructure.Services.RatingAndTopService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RatingAndTopController : BaseController
{
    private readonly IRatingAndTopService _ratingAndTopService;

    public RatingAndTopController(IRatingAndTopService ratingAndTopService)
    {
        _ratingAndTopService = ratingAndTopService;
    }
    [HttpGet("get/popularCategory"), AllowAnonymous]
    public async Task<ActionResult> PopularCategory([FromQuery] RatingAndTopFilter filter)
    {
        if (ModelState.IsValid)
        {
            var result = await _ratingAndTopService.PopularCategory(filter);
            return StatusCode(result.StatusCode, result);
        }
        var response = new Response<RatingAndTopDTO>(System.Net.HttpStatusCode.BadRequest, ModelStateErrors());
        return StatusCode(response.StatusCode, response);
    }

    [HttpGet("get/hotdicount"), AllowAnonymous]
    public async Task<ActionResult> HotDiscount([FromQuery] RatingAndTopFilter filter)
    {
        if (ModelState.IsValid)
        {
            var result = await _ratingAndTopService.HotDiscount(filter);
            return StatusCode(result.StatusCode, result);
        }
        var response =new Response<RatingAndTopDTO>(System.Net.HttpStatusCode.BadRequest, ModelStateErrors());
        return StatusCode(response.StatusCode, response);
    }
    [HttpGet("get/popularProduct"), AllowAnonymous]
    public async Task<ActionResult> PopularProduct([FromQuery] RatingAndTopFilter filter)
    {
        if (ModelState.IsValid)
        {
            var result = await _ratingAndTopService.PopularProduct(filter);
            return StatusCode(result.StatusCode, result);
        }
        var response = new Response<RatingAndTopDTO>(System.Net.HttpStatusCode.BadRequest, ModelStateErrors());
        return StatusCode(response.StatusCode, response);
    }

    [HttpGet("get/hitOfTheDay"), AllowAnonymous]
    public async Task<ActionResult> HitOfTheDay([FromQuery] RatingAndTopFilter filter)
    {
        if (ModelState.IsValid)
        {
            var result = await _ratingAndTopService.HitOfTheDay(filter);
            return StatusCode(result.StatusCode, result);
        }
        var response = new Response<RatingAndTopDTO>(System.Net.HttpStatusCode.BadRequest, ModelStateErrors());
        return StatusCode(response.StatusCode, response);
    }

    [HttpGet("get/hitOfTheMonth"), AllowAnonymous]
    public async Task<ActionResult> HitOfTheMonth([FromQuery] RatingAndTopFilter filter)
    {
        if (ModelState.IsValid)
        {
            var result = await _ratingAndTopService.HitOfTheMonth(filter);
            return StatusCode(result.StatusCode, result);
        }
        var response = new Response<RatingAndTopDTO>(System.Net.HttpStatusCode.BadRequest, ModelStateErrors());
        return StatusCode(response.StatusCode, response);
    }

    [HttpGet("get/hitOfTheYear"), AllowAnonymous]
    public async Task<ActionResult> HitOfTheYear([FromQuery] RatingAndTopFilter filter)
    {
        if (ModelState.IsValid)
        {
            var result = await _ratingAndTopService.HitOfTheYear(filter);
            return StatusCode(result.StatusCode, result);
        }
        var response = new Response<RatingAndTopDTO>(System.Net.HttpStatusCode.BadRequest, ModelStateErrors());
        return StatusCode(response.StatusCode, response);
    }

    // Personalized - no [AllowAnonymous]: BaseController requires auth by default, and this
    // reads the caller's own order history via CurrentUserId (the JWT "sid" claim), never a
    // client-supplied id, so there's no way to request another user's recommendations.
    [HttpGet("get/recommendedProduct")]
    public async Task<ActionResult> RecommendedProduct([FromQuery] RatingAndTopFilter filter)
    {
        if (ModelState.IsValid)
        {
            var result = await _ratingAndTopService.RecommendedProduct(CurrentUserId!, filter);
            return StatusCode(result.StatusCode, result);
        }
        var response = new Response<RatingAndTopDTO>(System.Net.HttpStatusCode.BadRequest, ModelStateErrors());
        return StatusCode(response.StatusCode, response);
    }
}
