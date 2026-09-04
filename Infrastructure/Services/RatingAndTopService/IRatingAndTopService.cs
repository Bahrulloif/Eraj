
using Domain.DTOs.RatingAndTopDTO;
using Domain.Filters.RatingAndTopFilter;
using Domain.Responses;

namespace Infrastructure.Services.RatingAndTopService;

public interface IRatingAndTopService
{
    public Task<PagedResponse<List<RatingAndTopDTO>>> PopularCategory(RatingAndTopFilter filter);
    public Task<PagedResponse<List<RatingAndTopDTO>>> HotDiscount(RatingAndTopFilter filter);
    // "You might also like" for the given user - not personalizable without a userId, so this
    // deliberately differs from the other RatingAndTopFilter-only signatures below it.
    public Task<PagedResponse<List<RatingAndTopDTO>>> RecommendedProduct(string userId, RatingAndTopFilter filter);
    public Task<PagedResponse<List<RatingAndTopDTO>>> PopularProduct(RatingAndTopFilter filter);
    public Task<PagedResponse<List<RatingAndTopDTO>>> HitOfTheYear(RatingAndTopFilter filter);
    public Task<PagedResponse<List<RatingAndTopDTO>>> HitOfTheMonth(RatingAndTopFilter filter);
    public Task<PagedResponse<List<RatingAndTopDTO>>> HitOfTheDay(RatingAndTopFilter filter);
}
