using AutoMapper;
using Domain.DTOs.RecommendationDTOs;
using Domain.Responses;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.RecommendationService;

public class RecommendationService : IRecommendationService
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;
    public RecommendationService(DataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;

    }
    public async Task<Response<List<RecommendationDto>>> GetRecommendation(string model)
    {
        var recommendationList = new List<RecommendationDto>();
        var noteBook = await _context.NoteBooks.Where(n => n.Model.ToLower().Contains(model.ToLower()))
        .Select(x => new RecommendationDto()
        {
            Model = x.Model,
            Color = x.Color,
            Price = x.Price,
            DiscountPrice = x.DiscountPrice,
            // Picture = x.Image

        }).AsNoTracking().ToListAsync();
        recommendationList.AddRange(noteBook);



        var smartPhone = await _context.SmartPhones.Where(s => s.Model.ToLower().Contains(model.ToLower()))
        .Select(y => new RecommendationDto()
        {
            Model = y.Model,
            Color = y.Color,
            Price = y.Price,
            DiscountPrice = y.DiscountPrice,
            // Picture = y.Picture
        }).AsNoTracking().ToListAsync();
        recommendationList.AddRange(smartPhone);


        var tablet = await _context.Tablets.Where(t => t.Model.ToLower().Contains(model.ToLower()))
        .Select(z => new RecommendationDto()
        {
            Model = z.Model,
            Color = z.Color,
            Price = z.Price,
            DiscountPrice = z.DiscountPrice,
            // Picture = z.Picture
        }).AsNoTracking().ToListAsync();
        recommendationList.AddRange(tablet);

        return new Response<List<RecommendationDto>>(recommendationList);
    }

}
