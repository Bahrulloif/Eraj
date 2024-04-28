using AutoMapper;
using Domain.DTOs.RatingAndTop;
using Domain.Entities.KompTech;
using Domain.Filters.RatingAndTopFilter;
using Domain.Responses;
using Infrastructure.Data;
using Infrastructure.Services.FileService;

namespace Infrastructure.Services.RatingAndTopService;

public class RatingAndTopService : IRatingAndTopService
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;
    private readonly IFileService _fileService;
    public RatingAndTopService(DataContext context, IMapper mapper, IFileService fileService)
    {
        _context = context;
        _fileService = fileService;
        _mapper = mapper;
    }
        public Task<PagedResponse<List<RatingAndTopDTO>>> PopularCategory(RatingAndTopFilter filter)
    {
        
    }
    public async Task<PagedResponse<List<RatingAndTopDTO>>> HotDiscount(RatingAndTopFilter filter)
    {
        var discount= (from s in _context.SmartPhones
        join
        )
    }
    public Task<PagedResponse<List<RatingAndTopDTO>>> RecommendedProduct(RatingAndTopFilter filter);
    public Task<PagedResponse<List<RatingAndTopDTO>>> PopularProduct(RatingAndTopFilter filter);
    public Task<PagedResponse<List<RatingAndTopDTO>>> HitOfTheYear(RatingAndTopFilter filter);
    public Task<PagedResponse<List<RatingAndTopDTO>>> HitOfTheMonth(RatingAndTopFilter filter);
    public Task<PagedResponse<List<RatingAndTopDTO>>> HitOfTheDay(RatingAndTopFilter filter);

}
