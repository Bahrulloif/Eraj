using Domain.DTOs.RecommendationDTOs;
using Domain.Responses;

namespace Infrastructure.Services.RecommendationService;

public interface IRecommendationService
{
    public Task<Response<List<RecommendationDto>>> GetRecommendation(string model);
}
