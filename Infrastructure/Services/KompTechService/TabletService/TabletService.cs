using AutoMapper;
using Domain.DTOs.KomTechDTOs.TabletDTOs;
using Domain.DTOs.PictureDTO;
using Domain.Entities;
using Domain.Entities.KompTech;
using Domain.Enum;
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
        // Skip/Take needs a deterministic order to paginate correctly - without it SQL doesn't
        // guarantee row order, so results can drift or duplicate across pages.
        query = query.OrderBy(x => x.Id);
        var mapped = await (from t in query
                            select new GetTabletDTO
                            {
                                Id = t.Id,
                                SubCategoryId = t.SubCategoryId,
                                Color = t.Color,
                                Core = t.Core,
                                Diagonal = t.Diagonal,
                                DiscountPrice = t.DiscountPrice,
                                Model = t.Model,
                                Price = t.Price,
                                RAM = t.RAM,
                                ROM = t.ROM,
                                Images = _context.Pictures.
                                    Where(p => p.ProductType == ProductType.Tablet && p.ProductId == t.Id && p.SubCategoryId == t.SubCategoryId).
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
        var mapped = await (from t in query
                            select new GetTabletDTO
                            {
                                Id = t.Id,
                                SubCategoryId = t.SubCategoryId,
                                Color = t.Color,
                                Core = t.Core,
                                Diagonal = t.Diagonal,
                                DiscountPrice = t.DiscountPrice,
                                Model = t.Model,
                                Price = t.Price,
                                RAM = t.RAM,
                                ROM = t.ROM,
                                Images = _context.Pictures.Where(p => p.ProductType == ProductType.Tablet && p.ProductId == t.Id && p.SubCategoryId == t.SubCategoryId).
                             Select(x => new PictureDto { ImageName = x.ImageName, Id = x.Id }).ToList()
                            }).FirstOrDefaultAsync();
        if (mapped == null)
        {
            return new Response<GetTabletDTO>(System.Net.HttpStatusCode.NotFound, "Tablet not found");
        }
        return new Response<GetTabletDTO>(mapped);
    }
    public async Task<Response<string>> AddTablet(AddTabletDTO tablet, string currentUserId)
    {
        if (tablet == null)
        {
            return new Response<string>(HttpStatusCode.NotFound, "Please fill the parameter");
        }
        var mapped = _mapper.Map<Tablet>(tablet);
        mapped.OwnerId = currentUserId;
        await _context.Tablets.AddAsync(mapped);
        await _context.SaveChangesAsync();
        foreach (var item in tablet.Images)
        {
            var imageName = _fileService.CreateFile(item);
            if (imageName.StatusCode != (int)HttpStatusCode.OK)
            {
                // Rejected (wrong type, corrupt, etc.) - skip it rather than insert a Picture
                // with a null ImageName, which would crash the whole request with an unhandled
                // DbUpdateException on the NOT NULL constraint.
                continue;
            }
            var image = new Picture
            {
                ImageName = imageName.Data!,
                ProductType = ProductType.Tablet,
                ProductId = mapped.Id,
                SubCategoryId = mapped.SubCategoryId
            };
            await _context.Pictures.AddAsync(image);
            await _context.SaveChangesAsync();
        }

        return new Response<string>($"{mapped.Model}Tablet added successfully");
    }
    public async Task<Response<string>> UpdateTablet(AddTabletDTO tablet, string currentUserId, bool isPrivileged)
    {
        if (tablet == null)
        {
            return new Response<string>(HttpStatusCode.NotFound, "Please fill parameter");
        }
        var find = await _context.Tablets.AsNoTracking().FirstOrDefaultAsync(t => t.Id == tablet.Id);
        if (find != null)
        {
            if (!isPrivileged && find.OwnerId != currentUserId)
            {
                return new Response<string>(HttpStatusCode.Forbidden, "You do not have access to this listing");
            }
            if (tablet.Images != null)
            {
                var images = await _context.Pictures.
                Where(x => x.ProductType == ProductType.Tablet && x.ProductId == find.Id && x.SubCategoryId == find.SubCategoryId).
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
                    if (imageName.StatusCode != (int)HttpStatusCode.OK)
                    {
                        continue;
                    }
                    var image = new Picture
                    {
                        ImageName = imageName.Data!,
                        ProductType = ProductType.Tablet,
                        ProductId = find.Id,
                        SubCategoryId = find.SubCategoryId
                    };
                    await _context.Pictures.AddAsync(image);
                    await _context.SaveChangesAsync();
                }
            }
            var mapped = _mapper.Map<Tablet>(tablet);
            mapped.OwnerId = find.OwnerId;
            _context.Tablets.Update(mapped);
            await _context.SaveChangesAsync();
            return new Response<string>("Tablet updated successfully");
        }
        return new Response<string>(HttpStatusCode.NotFound, "Tablet not found");
    }
    public async Task<Response<string>> DeleteTablet(int tabletId, string currentUserId, bool isPrivileged)
    {
        var find = await _context.Tablets.FirstOrDefaultAsync(t => t.Id == tabletId);
        if (find == null)
        {
            return new Response<string>(HttpStatusCode.NotFound, "Tablet not found");
        }
        if (!isPrivileged && find.OwnerId != currentUserId)
        {
            return new Response<string>(HttpStatusCode.Forbidden, "You do not have access to this listing");
        }
        var images = await _context.Pictures.
        Where(x => x.ProductType == ProductType.Tablet && x.ProductId == find.Id && x.SubCategoryId == find.SubCategoryId).
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
