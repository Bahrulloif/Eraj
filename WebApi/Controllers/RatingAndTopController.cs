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
    //   public Task<PagedResponse<List<RatingAndTopDTO>>> PopularCategory(RatingAndTopFilter filter);
    
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
    // public Task<PagedResponse<List<RatingAndTopDTO>>> RecommendedProduct(RatingAndTopFilter filter);
    // public Task<PagedResponse<List<RatingAndTopDTO>>> PopularProduct(RatingAndTopFilter filter);
    // public Task<PagedResponse<List<RatingAndTopDTO>>> HitOfTheYear(RatingAndTopFilter filter);
    // public Task<PagedResponse<List<RatingAndTopDTO>>> HitOfTheMonth(RatingAndTopFilter filter);
    // public Task<PagedResponse<List<RatingAndTopDTO>>> HitOfTheDay(RatingAndTopFilter filter);
}
