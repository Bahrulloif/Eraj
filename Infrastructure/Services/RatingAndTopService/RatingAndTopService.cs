using AutoMapper;
using Domain.DTOs.PictureDTO;
using Domain.DTOs.RatingAndTopDTO;
using Domain.DTOs.RatingAndTopDTOs;
using Domain.Entities;
using Domain.Enum;
using Domain.Filters.RatingAndTopFilter;
using Domain.Responses;
using Infrastructure.Data;
using Infrastructure.Services.FileService;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Linq.Expressions;
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
    // Shared by every "top ordered products" variant below (PopularCategory, PopularProduct,
    // HitOfThe*, RecommendedProduct): rank products by total quantity ordered, grouped by
    // (ProductId, SubCategoryId) - one row per distinct product, not per order - and paginate
    // over those groups rather than the raw Orders rows. `where`, when given, narrows which
    // Orders rows are counted (a date range, a subcategory/product filter, etc.) before grouping.
    //
    // NOTE: Orders (unlike the 11 product tables) has no product-type discriminator, so the
    // Pictures lookup below can't filter by ProductType and keeps relying on
    // (ProductId, SubCategoryId) alone - same collision risk Picture had before ProductType was
    // added elsewhere. Would need ProductType added to Order too.
    private async Task<PagedResponse<List<RatingAndTopDTO>>> GetTopOrderedProducts(
        RatingAndTopFilter filter, Expression<Func<Order, bool>>? where = null)
    {
        var query = _context.Orders.AsQueryable();
        if (where != null)
        {
            query = query.Where(where);
        }

        var groupedQuery = query
            .GroupBy(o => new { o.ProductId, o.SubCategoryId })
            .OrderByDescending(g => g.Sum(o => o.Quantity));

        var totalRecord = await groupedQuery.CountAsync();

        var list = await groupedQuery
            .Skip((filter.PageNumber - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .Select(result => new RatingAndTopDTO
            {
                ProductId = result.Key.ProductId,
                SubCategoryId = result.Key.SubCategoryId,
                Model = result.Select(i => i.Model).First(),
                Price = result.Select(i => i.Price).First(),
                Images = _context.Pictures.Where(i => i.ProductId == result.Key.ProductId && i.SubCategoryId == result.Key.SubCategoryId)
                    .Select(z => new PictureDto { Id = z.Id, ImageName = z.ImageName }).ToList()
            }).ToListAsync();

        return new PagedResponse<List<RatingAndTopDTO>>(list, filter.PageNumber, filter.PageSize, totalRecord);
    }

    public Task<PagedResponse<List<RatingAndTopDTO>>> PopularCategory(RatingAndTopFilter filter) =>
        GetTopOrderedProducts(filter);

    // Same ranking as PopularCategory - RatingAndTopDTO is product-shaped (Model/Price/Images),
    // not category-shaped, and PopularCategory already groups by individual product, so there's
    // no distinct "category-level" query this could be beyond a duplicate of PopularCategory.
    public Task<PagedResponse<List<RatingAndTopDTO>>> PopularProduct(RatingAndTopFilter filter) =>
        GetTopOrderedProducts(filter);

    public Task<PagedResponse<List<RatingAndTopDTO>>> HitOfTheDay(RatingAndTopFilter filter) =>
        GetTopOrderedProducts(filter, o => o.OrderDate >= DateTime.UtcNow.Date);

    public Task<PagedResponse<List<RatingAndTopDTO>>> HitOfTheMonth(RatingAndTopFilter filter)
    {
        var now = DateTime.UtcNow;
        var startOfMonth = new DateTime(now.Year, now.Month, 1, 0, 0, 0, DateTimeKind.Utc);
        return GetTopOrderedProducts(filter, o => o.OrderDate >= startOfMonth);
    }

    public Task<PagedResponse<List<RatingAndTopDTO>>> HitOfTheYear(RatingAndTopFilter filter)
    {
        var now = DateTime.UtcNow;
        var startOfYear = new DateTime(now.Year, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        return GetTopOrderedProducts(filter, o => o.OrderDate >= startOfYear);
    }

    // "You might also like": popular products from subcategories this user has ordered from
    // before, excluding products they've already ordered themselves. Falls back to an empty
    // page (not an error) for a user with no order history yet - there's nothing to base a
    // recommendation on.
    public async Task<PagedResponse<List<RatingAndTopDTO>>> RecommendedProduct(string userId, RatingAndTopFilter filter)
    {
        var orderedSubCategoryIds = await _context.Orders
            .Where(o => o.ApplicationUserId == userId)
            .Select(o => o.SubCategoryId)
            .Distinct()
            .ToListAsync();

        if (orderedSubCategoryIds.Count == 0)
        {
            return new PagedResponse<List<RatingAndTopDTO>>(new List<RatingAndTopDTO>(), filter.PageNumber, filter.PageSize, 0);
        }

        var alreadyOrderedProductIds = await _context.Orders
            .Where(o => o.ApplicationUserId == userId)
            .Select(o => o.ProductId)
            .Distinct()
            .ToListAsync();

        return await GetTopOrderedProducts(filter, o =>
            orderedSubCategoryIds.Contains(o.SubCategoryId) && !alreadyOrderedProductIds.Contains(o.ProductId));
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
}
