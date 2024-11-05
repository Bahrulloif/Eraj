using AutoMapper;
using Domain.DTOs.PictureDTO;
using Domain.DTOs.RatingAndTopDTO;
using Domain.DTOs.RatingAndTopDTOs;
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
        List<RatingAndTopDTO> popularCategory = new List<RatingAndTopDTO>();
        var popularList1 = await _context.Orders
        .GroupBy(i => new { i.ProductId, i.SubCategoryId })
        .OrderByDescending(y => y.Sum(t => t.Quantity))
        .Select(x => new RatingAndTopDTO
        {
            ProductId = x.Select(z => z.ProductId).First(),

        }).ToListAsync();
        var query = _context.Orders.AsQueryable();
        var popularList = await (from data in query
                                 group data by new { data.ProductId, data.SubCategoryId } into result
                                 orderby result.Sum(i => i.Quantity) descending
                                 select new RatingAndTopDTO
                                 {
                                     ProductId = result.Key.ProductId,
                                     SubCategoryId = result.Key.SubCategoryId,
                                     Model = result.Select(i => i.Model).First(),
                                     Price = result.Select(i => i.Price).First(),
                                     Images = _context.Pictures.Where(i => i.ProductId == result.Key.ProductId && i.SubCategoryId == result.Key.SubCategoryId)
                                     .Select(z => new PictureDto { Id = z.Id, ImageName = z.ImageName }).ToList()
                                 }).Take(20).ToListAsync();
        // foreach (var item in popularList)
        // {
        //     var a = new RatingAndTopDTO();
        //     a.ProductId = item.ProductId;
        //     a.SubCategoryId = item.SubCategoryId;
        //     a.Model = (await from s in _context.SmartPhones
        //                      join n in _context.NoteBooks on s.Id equals n.Id into table1
        //                      from t1 in table1
        //                      join t in _context.Tablets on t1.Id equals t.Id into table2
        //                      from t2 in table2
        //                      join sakomp in _context.SpareAccessorKomps on t2.Id equals sakomp.Id into table3
        //                      from t3 in table3
        //                      join c in _context.Cars on t3.Id equals c.Id into table4
        //                      from t4 in table4
        //                      join m in _context.Motorbikes on t4.Id equals m.Id into table5
        //                      from t5 in table5
        //                      join tr in _context.Trucks on t5.Id equals tr.Id into table6
        //                      from t6 in table6
        //                      join satran in _context.SpareAccessorTransps on t6.Id equals satran.Id into table7
        //                      from t7 in table7
        //                      where t7.Id == item.Id && t7.SubCategoryId == item.SubCategoryId
        //                      select t7.Model).FirstOrDefaultAsync();
        // }
        var totalRecord = query.Count();
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
            DiscountPrice = z.DiscountPrice,
            Model = z.Model,
            Price = z.Price,
            Images = _context.Pictures
                .Where(x => x.ProductId == z.Id && x.SubCategoryId == z.SubCategoryId)
                .Select(y => new PictureDto { Id = y.ProductId, ImageName = y.ImageName }).ToList()
        }).ToListAsync();
        hotdiscount.AddRange(smart);


        var noteBook = await _context.NoteBooks
        .Where(x => x.DiscountPrice >= 50)
        .OrderByDescending(t => t.DiscountPrice)
        .Select(z => new RatingAndTopDTO
        {
            ProductId = z.Id,
            DiscountPrice = z.DiscountPrice,
            Model = z.Model,
            Price = z.Price,
            Images = _context.Pictures
                .Where(x => x.ProductId == z.Id && x.SubCategoryId == z.SubCategoryId)
                .Select(y => new PictureDto { Id = y.ProductId, ImageName = y.ImageName }).ToList()
        }).ToListAsync();
        hotdiscount.AddRange(noteBook);

        var tablet = await _context.Tablets
        .Where(x => x.DiscountPrice >= 50)
        .OrderByDescending(t => t.DiscountPrice)
        .Select(z => new RatingAndTopDTO
        {
            ProductId = z.Id,
            DiscountPrice = z.DiscountPrice,
            Model = z.Model,
            Price = z.Price,
            Images = _context.Pictures
          .Where(x => x.ProductId == z.Id && x.SubCategoryId == z.SubCategoryId)
          .Select(y => new PictureDto { Id = y.ProductId, ImageName = y.ImageName }).ToList()
        }).ToListAsync();
        hotdiscount.AddRange(tablet);

        var spareAccessorKomp = await _context.SpareAccessorKomps
        .Where(x => x.DiscountPrice >= 50)
        .OrderByDescending(t => t.DiscountPrice)
        .Select(z => new RatingAndTopDTO
        {
            ProductId = z.Id,
            DiscountPrice = z.DiscountPrice,
            Model = z.Model,
            Price = z.Price,
            Images = _context.Pictures
           .Where(x => x.ProductId == z.Id && x.SubCategoryId == z.SubCategoryId)
           .Select(y => new PictureDto { Id = y.ProductId, ImageName = y.ImageName })
           .ToList()
        }).ToListAsync();
        hotdiscount.AddRange(spareAccessorKomp);

        var car = await _context.Cars
        .Where(x => x.DiscountPrice >= 10)
        .OrderByDescending(t => t.DiscountPrice)
        .Select(z => new RatingAndTopDTO
        {
            ProductId = z.Id,
            DiscountPrice = z.DiscountPrice,
            Model = z.Model,
            Price = z.Price,
            Images = _context.Pictures
            .Where(x => x.ProductId == z.Id && x.SubCategoryId == z.SubCategoryId)
            .Select(y => new PictureDto { Id = y.ProductId, ImageName = y.ImageName })
            .ToList()
        }).ToListAsync();
        hotdiscount.AddRange(car);

        



        hotdiscount = hotdiscount.OrderByDescending(r => r.DiscountPrice).ToList();
        var totalCount = hotdiscount.Count();
        return new PagedResponse<List<RatingAndTopDTO>>(hotdiscount, filter.PageNumber, filter.PageSize, totalCount);
    }
    // public Task<PagedResponse<List<RatingAndTopDTO>>> RecommendedProduct(RatingAndTopFilter filter);
    // public Task<PagedResponse<List<RatingAndTopDTO>>> PopularProduct(RatingAndTopFilter filter);
    // public Task<PagedResponse<List<RatingAndTopDTO>>> HitOfTheYear(RatingAndTopFilter filter);
    // public Task<PagedResponse<List<RatingAndTopDTO>>> HitOfTheMonth(RatingAndTopFilter filter);
    // public Task<PagedResponse<List<RatingAndTopDTO>>> HitOfTheDay(RatingAndTopFilter filter);

}
