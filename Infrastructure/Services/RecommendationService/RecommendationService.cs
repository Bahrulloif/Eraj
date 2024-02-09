using AutoMapper;
using Domain.DTOs.PictureDTO;
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
            Image = _context.Pictures.
            Where(z => z.ProductId == x.Id && z.SubCategoryId == x.SubCategoryId).
            Select(y => new PictureDto{Id = y.Id, ImageName = y.ImageName}).
            ToList()
        }).AsNoTracking().ToListAsync();
        recommendationList.AddRange(noteBook);



        var smartPhone = await _context.SmartPhones.Where(s => s.Model.ToLower().Contains(model.ToLower()))
        .Select(y => new RecommendationDto()
        {
            Model = y.Model,
            Color = y.Color,
            Price = y.Price,
            DiscountPrice = y.DiscountPrice,
            Image = _context.Pictures.
            Where(z => z.ProductId == y.Id && z.SubCategoryId == y.SubCategoryId).
            Select(a => new PictureDto{Id = a.Id, ImageName = a.ImageName}).
            ToList()
        }).AsNoTracking().ToListAsync();
        recommendationList.AddRange(smartPhone);


        var tablet = await _context.Tablets.Where(t => t.Model.ToLower().Contains(model.ToLower()))
        .Select(z => new RecommendationDto()
        {
            Model = z.Model,
            Color = z.Color,
            Price = z.Price,
            DiscountPrice = z.DiscountPrice,
            Image = _context.Pictures.
            Where(a => a.ProductId == z.Id && a.SubCategoryId == z.SubCategoryId).
            Select(y => new PictureDto{Id = y.Id, ImageName = y.ImageName}).
            ToList()
        }).AsNoTracking().ToListAsync();
        recommendationList.AddRange(tablet);

        return new Response<List<RecommendationDto>>(recommendationList);
    }

}
