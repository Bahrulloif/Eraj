using AutoMapper;
using Domain.DTOs.KomTechDTOs.TabletDTOs;
using Domain.DTOs.PictureDTO;
using Domain.Entities;
using Domain.Entities.KompTech;
using Domain.Filters.KompTechFilters.TabletFilter;
using Domain.Responses;
using Infrastructure.Data;
using Infrastructure.Services.FileService;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Infrastructure.Services.KompTechService.TabletService;

public class TabletService : ITabletService
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;
    private readonly IFileService _fileService;
    public TabletService(DataContext context, IMapper mapper, IFileService fileService)
    {
        _context = context;
        _mapper = mapper;
        _fileService = fileService;
    }
    public async Task<PagedResponse<List<GetTabletDTO>>> GetTablet(GetTabletFilter filter)
    {
        var query = _context.Tablets.AsQueryable();
        if (filter.Name != null)
        {
            query = query.Where(t => t.Model.ToLower().Contains(filter.Name.ToLower()));
        }
        var mapped = await (from t in query
                            select new GetTabletDTO
                            {
                                Id = t.Id,
                                Color = t.Color,
                                Core = t.Core,
                                Diagonal = t.Diagonal,
                                DiscountPrice = t.DiscountPrice,
                                Model = t.Model,
                                Price = t.Price,
                                RAM = t.RAM,
                                ROM = t.ROM,
                                Images = _context.Pictures.
                                    Where(p => p.ProductId == t.Id && p.SubCategoryId == t.SubCategoryId).
                                    Select(x => new PictureDto { Id = x.Id, ImageName = x.ImageName }).
                                    ToList()
                            }).Skip((filter.PageNumber - 1) * filter.PageSize).
                            Take(filter.PageSize).ToListAsync();
        var totalCount = await query.CountAsync();
        return new PagedResponse<List<GetTabletDTO>>(mapped, filter.PageNumber, filter.PageSize, totalCount);
    }

    public async Task<Response<GetTabletDTO>> GetTabletById(int tabletId)
    {
        var query = _context.Tablets.AsQueryable();
        query = query.Where(t => t.Id == tabletId);
        if (query == null)
        {
            return new Response<GetTabletDTO>(HttpStatusCode.NotFound, "Tablet not found");
        }

        var mapped = await (from t in query
                            select new GetTabletDTO
                            {
                                Id = t.Id,
                                Color = t.Color,
                                Core = t.Core,
                                Diagonal = t.Diagonal,
                                DiscountPrice = t.DiscountPrice,
                                Model = t.Model,
                                Price = t.Price,
                                RAM = t.RAM,
                                ROM = t.ROM,
                                Images = _context.Pictures.Where(p => p.ProductId == t.Id && p.SubCategoryId == t.SubCategoryId).
                             Select(x => new PictureDto { ImageName = x.ImageName, Id = x.Id }).ToList()
                            }).FirstOrDefaultAsync();
        return new Response<GetTabletDTO>(mapped);
    }
    public async Task<Response<string>> AddTablet(AddTabletDTO tablet)
    {
        if (tablet == null)
        {
            return new Response<string>(HttpStatusCode.NotFound, "Please fill the parameter");
        }
        var mapped = _mapper.Map<Tablet>(tablet);
        await _context.Tablets.AddAsync(mapped);
        foreach (var item in tablet.Images)
        {
            var imageName = _fileService.CreateFile(item);
            var image = new Picture
            {
                ImageName = imageName.Data!,
                ProductId = mapped.Id,
                SubCategoryId = mapped.SubCategoryId
            };
            await _context.Pictures.AddAsync(image);
            await _context.SaveChangesAsync();
        }

        return new Response<string>($"{mapped.Model}Tablet added successfully");
    }
    public async Task<Response<string>> UpdateTablet(AddTabletDTO tablet)
    {
        if (tablet == null)
        {
            return new Response<string>(HttpStatusCode.NotFound, "Please fill parameter");
        }
        var find = await _context.Tablets.AsNoTracking().FirstOrDefaultAsync(t => t.Id == tablet.Id);
        if (find != null)
        {
            if (tablet.Images != null)
            {
                var images = await _context.Pictures.
                Where(x => x.ProductId == find.Id && x.SubCategoryId == find.SubCategoryId).
                ToListAsync();
                foreach (var item in images)
                {
                    _fileService.DeleteFile(item.ImageName);
                }
                _context.Pictures.RemoveRange(images);
                await _context.SaveChangesAsync();
                foreach (var item in tablet.Images)
                {
                    var imageName = _fileService.CreateFile(item);
                    var image = new Picture
                    {
                        ImageName = imageName.Data!,
                        ProductId = find.Id,
                        SubCategoryId = find.SubCategoryId
                    };
                    await _context.Pictures.AddAsync(image);
                    await _context.SaveChangesAsync();
                }
            }
            var mapped = _mapper.Map<Tablet>(find);
            _context.Tablets.Update(mapped);
            await _context.SaveChangesAsync();
            return new Response<string>("Tablet updated successfully");
        }
        return new Response<string>(HttpStatusCode.NotFound, "Tablet not found");
    }
    public async Task<Response<string>> DeleteTablet(int tabletId)
    {
        var find = await _context.Tablets.FirstOrDefaultAsync(t => t.Id == tabletId);
        if (find == null)
        {
            return new Response<string>(HttpStatusCode.NotFound, "Tablet not found");
        }
        var images = await _context.Pictures.
        Where(x => x.ProductId == find.Id && x.SubCategoryId == find.SubCategoryId).
        ToListAsync();

        foreach (var item in images)
        {
            _fileService.DeleteFile(item.ImageName);
        }
        _context.Pictures.RemoveRange(images);
        _context.Tablets.Remove(find);
        await _context.SaveChangesAsync();
        return new Response<string>("Tablet deleted successfully");
    }
}
