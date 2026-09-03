using AutoMapper;
using Domain.DTOs.PictureDTO;
using Domain.DTOs.RatingAndTopDTO;
using Domain.DTOs.RatingAndTopDTOs;
using Domain.Enum;
using Domain.Filters.RatingAndTopFilter;
using Domain.Responses;
using Infrastructure.Data;
using Infrastructure.Services.FileService;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
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
    public async Task<PagedResponse<List<RatingAndTopDTO>>> PopularCategory(RatingAndTopFilter filter)
    {
        // Ranked by total quantity ordered, grouped by (ProductId, SubCategoryId) - one row
        // per distinct product, not per order. Pagination has to operate on these groups,
        // not on the raw Orders rows.
        var groupedQuery = _context.Orders
            .GroupBy(o => new { o.ProductId, o.SubCategoryId })
            .OrderByDescending(g => g.Sum(o => o.Quantity));

        var totalRecord = await groupedQuery.CountAsync();

        var popularList = await groupedQuery
            .Skip((filter.PageNumber - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .Select(result => new RatingAndTopDTO
            {
                ProductId = result.Key.ProductId,
                SubCategoryId = result.Key.SubCategoryId,
                Model = result.Select(i => i.Model).First(),
                Price = result.Select(i => i.Price).First(),
                // NOTE: Orders (unlike the 11 product tables) has no product-type discriminator,
                // so this lookup can't filter by ProductType and keeps relying on
                // (ProductId, SubCategoryId) alone - same collision risk Picture had before
                // ProductType was added elsewhere. Would need ProductType added to Order too.
                Images = _context.Pictures.Where(i => i.ProductId == result.Key.ProductId && i.SubCategoryId == result.Key.SubCategoryId)
                    .Select(z => new PictureDto { Id = z.Id, ImageName = z.ImageName }).ToList()
            }).ToListAsync();

        return new PagedResponse<List<RatingAndTopDTO>>(popularList, filter.PageNumber, filter.PageSize, totalRecord);
    }

    public async Task<PagedResponse<List<RatingAndTopDTO>>> HotDiscount(RatingAndTopFilter filter)
    {
        List<RatingAndTopDTO> hotdiscount = new List<RatingAndTopDTO>();

        // var leftDiscount = await (from s in _context.SmartPhones
        //                           join n in _context.NoteBooks on s.Id equals n.Id into intertable
        //                           from i in intertable
        //                           join t in _context.Tablets on i.Id equals t.Id into general
        //                           from g in general.DefaultIfEmpty()
        //                           select new RatingAndTopDTO
        //                           {
        //                               ProductId = g.Id,
        //                               SubCategoryId = g.SubCategoryId,
        //                               Model = g.Model,
        //                               DiscountPrice = g.DiscountPrice,
        //                               Price = g.Price,
        //                               Images = _context.Pictures
        //                               .Where(x => x.ProductId == g.Id && x.SubCategoryId == g.SubCategoryId)
        //                               .Select(z => new PictureDto { Id = z.Id, ImageName = z.ImageName })
        //                               .ToList()
        //                           }).OrderByDescending(t => t.DiscountPrice).ToListAsync();


        // var rightDiscount = await (from n in _context.NoteBooks
        //                            join s in _context.SmartPhones on n.Id equals s.Id into intertable
        //                            from i in intertable
        //                            join t in _context.Tablets on i.Id equals t.Id into general
        //                            from g in general.DefaultIfEmpty()
        //                            select new RatingAndTopDTO
        //                            {
        //                                ProductId = g.Id,
        //                                SubCategoryId = g.SubCategoryId,
        //                                Model = g.Model,
        //                                DiscountPrice = g.DiscountPrice,
        //                                Price = g.Price,
        //                                Images = _context.Pictures
        //                                .Where(x => x.ProductId == g.Id && x.SubCategoryId == g.SubCategoryId)
        //                                .Select(z => new PictureDto { Id = z.Id, ImageName = z.ImageName })
        //                                .ToList()
        //                            }).OrderByDescending(t => t.DiscountPrice).
        //                            ToListAsync();

        // var fullOuterDiscount = leftDiscount.Union(rightDiscount);
        // hotdiscount = fullOuterDiscount.OrderByDescending(t => t.DiscountPrice).ToList();



        var smart = await _context.SmartPhones
        .Where(x => x.DiscountPrice >= 10)
        .OrderByDescending(t => t.DiscountPrice)
        .Select(z => new RatingAndTopDTO
        {
            ProductId = z.Id,
            SubCategoryId = z.SubCategoryId,
            DiscountPrice = z.DiscountPrice,
            Model = z.Model,
            Price = z.Price,
            Images = _context.Pictures
                .Where(x => x.ProductType == ProductType.SmartPhone && x.ProductId == z.Id && x.SubCategoryId == z.SubCategoryId)
                .Select(y => new PictureDto { Id = y.Id, ImageName = y.ImageName }).ToList()
        }).ToListAsync();
        hotdiscount.AddRange(smart);


        var noteBook = await _context.NoteBooks
        .Where(x => x.DiscountPrice >= 50)
        .OrderByDescending(t => t.DiscountPrice)
        .Select(z => new RatingAndTopDTO
        {
            ProductId = z.Id,
            SubCategoryId = z.SubCategoryId,
            DiscountPrice = z.DiscountPrice,
            Model = z.Model,
            Price = z.Price,
            Images = _context.Pictures
                .Where(x => x.ProductType == ProductType.NoteBook && x.ProductId == z.Id && x.SubCategoryId == z.SubCategoryId)
                .Select(y => new PictureDto { Id = y.Id, ImageName = y.ImageName }).ToList()
        }).ToListAsync();
        hotdiscount.AddRange(noteBook);

        var tablet = await _context.Tablets
        .Where(x => x.DiscountPrice >= 50)
        .OrderByDescending(t => t.DiscountPrice)
        .Select(z => new RatingAndTopDTO
        {
            ProductId = z.Id,
            SubCategoryId = z.SubCategoryId,
            DiscountPrice = z.DiscountPrice,
            Model = z.Model,
            Price = z.Price,
            Images = _context.Pictures
          .Where(x => x.ProductType == ProductType.Tablet && x.ProductId == z.Id && x.SubCategoryId == z.SubCategoryId)
          .Select(y => new PictureDto { Id = y.Id, ImageName = y.ImageName }).ToList()
        }).ToListAsync();
        hotdiscount.AddRange(tablet);

        var spareAccessorKomp = await _context.SpareAccessorKomps
        .Where(x => x.DiscountPrice >= 50)
        .OrderByDescending(t => t.DiscountPrice)
        .Select(z => new RatingAndTopDTO
        {
            ProductId = z.Id,
            SubCategoryId = z.SubCategoryId,
            DiscountPrice = z.DiscountPrice,
            Model = z.Model,
            Price = z.Price,
            Images = _context.Pictures
           .Where(x => x.ProductType == ProductType.SpareAccessorKomp && x.ProductId == z.Id && x.SubCategoryId == z.SubCategoryId)
           .Select(y => new PictureDto { Id = y.Id, ImageName = y.ImageName })
           .ToList()
        }).ToListAsync();
        hotdiscount.AddRange(spareAccessorKomp);

        var car = await _context.Cars
        .Where(x => x.DiscountPrice >= 10)
        .OrderByDescending(t => t.DiscountPrice)
        .Select(z => new RatingAndTopDTO
        {
            ProductId = z.Id,
            SubCategoryId = z.SubCategoryId,
            DiscountPrice = z.DiscountPrice,
            Model = z.Model,
            Price = z.Price,
            Images = _context.Pictures
            .Where(x => x.ProductType == ProductType.Car && x.ProductId == z.Id && x.SubCategoryId == z.SubCategoryId)
            .Select(y => new PictureDto { Id = y.Id, ImageName = y.ImageName })
            .ToList()
        }).ToListAsync();
        hotdiscount.AddRange(car);

        



        hotdiscount = hotdiscount.OrderByDescending(r => r.DiscountPrice).ToList();
        var totalCount = hotdiscount.Count();
        var paged = hotdiscount.Skip((filter.PageNumber - 1) * filter.PageSize).Take(filter.PageSize).ToList();
        return new PagedResponse<List<RatingAndTopDTO>>(paged, filter.PageNumber, filter.PageSize, totalCount);
    }
    // public Task<PagedResponse<List<RatingAndTopDTO>>> RecommendedProduct(RatingAndTopFilter filter);
    // public Task<PagedResponse<List<RatingAndTopDTO>>> PopularProduct(RatingAndTopFilter filter);
    // public Task<PagedResponse<List<RatingAndTopDTO>>> HitOfTheYear(RatingAndTopFilter filter);
    // public Task<PagedResponse<List<RatingAndTopDTO>>> HitOfTheMonth(RatingAndTopFilter filter);
    // public Task<PagedResponse<List<RatingAndTopDTO>>> HitOfTheDay(RatingAndTopFilter filter);

}
