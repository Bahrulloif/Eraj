using Domain.DTOs.RecommendationDTOs;
using Domain.Responses;
using Infrastructure.Services.RecommendationService;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RecommendationController : BaseController
{
    private readonly IRecommendationService _recommendationService;
    public RecommendationController(IRecommendationService recommendationService)
    {
        _recommendationService = recommendationService;
    }

    [HttpGet("get/recommendation")]
    public async Task<IActionResult> GetRecommendation(string model)
    {
        if (ModelState.IsValid)
        {
            var result = await _recommendationService.GetRecommendation(model);
            return StatusCode(result.StatusCode, result);
        }
        var response = new Response<List<RecommendationDto>>(System.Net.HttpStatusCode.BadRequest, ModelStateErrors());
        return StatusCode(response.StatusCode, response);
    }

}
