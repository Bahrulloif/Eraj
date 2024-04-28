using Domain.DTOs.RatingAndTop;
using Domain.Filters.RatingAndTopFilter;
using Domain.Responses;

namespace Infrastructure.Services.IRatingAndTopService;

public interface IRatingAndTopService
{
    public Task<PagedResponse<List<RatingAndTopDTO>>> PopularCategory(RatingAndTopFilter filter);
    public Task<PagedResponse<List<RatingAndTopDTO>>> HotDiscount(RatingAndTopFilter filter);
    public Task<PagedResponse<List<RatingAndTopDTO>>> RecommendedProduct(RatingAndTopFilter filter);
    public Task<PagedResponse<List<RatingAndTopDTO>>> PopularProduct(RatingAndTopFilter filter);
    public Task<PagedResponse<List<RatingAndTopDTO>>> HitOfTheYear(RatingAndTopFilter filter);
    public Task<PagedResponse<List<RatingAndTopDTO>>> HitOfTheMonth(RatingAndTopFilter filter);
    public Task<PagedResponse<List<RatingAndTopDTO>>> HitOfTheDay(RatingAndTopFilter filter);
}
