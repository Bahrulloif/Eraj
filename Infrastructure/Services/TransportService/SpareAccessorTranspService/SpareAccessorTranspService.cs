using AutoMapper;
using Domain.DTOs.PictureDTO;
using Domain.DTOs.TransportDTOs.SpareAccessorTranspDTOs;
using Domain.Entities;
using Domain.Entities.Transport;
using Domain.Enum;
using Domain.Filters.TransportFilter.SpareAccessorTranspFilters;
using Domain.Responses;
using Infrastructure.Data;
using Infrastructure.Services.FileService;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.TransportService.SpareAccessorTranspService;

public class SpareAccessorTranspService : ISpareAccessorTranspService
{
    private readonly IMapper _mapper;
    private readonly DataContext _context;
    private readonly IFileService _fileService;
    public SpareAccessorTranspService(IMapper mapper, DataContext context, IFileService fileService)
    {
        _context = context;
        _mapper = mapper;
        _fileService = fileService;
    }

    public async Task<PagedResponse<List<GetSpareAccessorTranspDTO>>> GetSpareAccessorTransp(GetSpareAccessorTranspFilter filter)
    {
        var query = _context.SpareAccessorTransps.AsQueryable();
        if (filter.Model != null)
        {
            query = query.Where(x => x.Model.ToLower().Contains(filter.Model.ToLower()));
        }
        // Skip/Take needs a deterministic order to paginate correctly - without it SQL doesn't
        // guarantee row order, so results can drift or duplicate across pages.
        query = query.OrderBy(x => x.Id);
        var mapped = await (from a in query
                            select new GetSpareAccessorTranspDTO
                            {
                                Id = a.Id,
                                SubCategoryId = a.SubCategoryId,
                                Model = a.Model,
                                Description = a.Description,
                                Price = a.Price,
                                DiscountPrice = a.DiscountPrice,
                                Images = _context.Pictures
                                    .Where(p => p.ProductType == ProductType.SpareAccessorTransp && p.ProductId == a.Id && p.SubCategoryId == a.SubCategoryId)
                                    .Select(s => new PictureDto { Id = s.Id, ImageName = s.ImageName })
                                    .ToList()
                            }).Skip((filter.PageNumber - 1) * filter.PageSize)
                            .Take(filter.PageSize).ToListAsync();
        var totalRecord = query.Count();
        return new PagedResponse<List<GetSpareAccessorTranspDTO>>(mapped, filter.PageNumber, filter.PageSize, totalRecord);
    }

    public async Task<Response<GetSpareAccessorTranspDTO>> GetSpareAccessorTranspById(int spareAccessorTranspId)
    {
        var query = _context.SpareAccessorTransps.AsQueryable();
        query = query.Where(x => x.Id == spareAccessorTranspId);
        var mapped = await (from a in query
                            select new GetSpareAccessorTranspDTO
                            {
                                Id = a.Id,
                                SubCategoryId = a.SubCategoryId,
                                Model = a.Model,
                                Description = a.Description,
                                Price = a.Price,
                                DiscountPrice = a.DiscountPrice,
                                Images = _context.Pictures
                                    .Where(p => p.ProductType == ProductType.SpareAccessorTransp && p.ProductId == a.Id && p.SubCategoryId == a.SubCategoryId)
                                    .Select(s => new PictureDto { Id = s.Id, ImageName = s.ImageName })
                                    .ToList()
                            }).FirstOrDefaultAsync();
        if (mapped == null)
        {
            return new Response<GetSpareAccessorTranspDTO>(System.Net.HttpStatusCode.NotFound, "SpareAccessorTransp not found");
        }
        return new Response<GetSpareAccessorTranspDTO>(mapped);
    }

    public async Task<Response<string>> AddSpareAccessorTransp(AddSpareAccessorTranspDTO spareAccessorTransp, string currentUserId)
    {
        var mapped = _mapper.Map<SpareAccessorTransp>(spareAccessorTransp);
        mapped.OwnerId = currentUserId;
        await _context.SpareAccessorTransps.AddAsync(mapped);
        await _context.SaveChangesAsync();
        if (spareAccessorTransp.Images != null)
        {
            foreach (var item in spareAccessorTransp.Images)
            {
                var imageName = _fileService.CreateFile(item);
                if (imageName.StatusCode != (int)System.Net.HttpStatusCode.OK)
                {
                    // Rejected (wrong type, corrupt, etc.) - skip it rather than insert a
                    // Picture with a null ImageName, which would crash the whole request with an
                    // unhandled DbUpdateException on the NOT NULL constraint.
                    continue;
                }
                var image = new Picture
                {
                    ImageName = imageName.Data!,
                    ProductType = ProductType.SpareAccessorTransp,
                    ProductId = mapped.Id,
                    SubCategoryId = spareAccessorTransp.SubCategoryId
                };
                await _context.Pictures.AddAsync(image);
                await _context.SaveChangesAsync();
            }
        }
        return new Response<string>(System.Net.HttpStatusCode.OK, $"{spareAccessorTransp.Model} added successfully");
    }

    public async Task<Response<string>> UpdateSpareAccessorTransp(AddSpareAccessorTranspDTO spareAccessorTransp, string currentUserId, bool isPrivileged)
    {
        var find = await _context.SpareAccessorTransps.FirstOrDefaultAsync(x => x.Id == spareAccessorTransp.Id);
        if (find != null)
        {
            if (!isPrivileged && find.OwnerId != currentUserId)
            {
                return new Response<string>(System.Net.HttpStatusCode.Forbidden, "You do not have access to this listing");
            }
            _mapper.Map(spareAccessorTransp, find);
            _context.SpareAccessorTransps.Update(find);
            await _context.SaveChangesAsync();
            if (spareAccessorTransp.Images != null)
            {
                var images = await _context.Pictures.Where(x => x.ProductType == ProductType.SpareAccessorTransp && x.ProductId == spareAccessorTransp.Id && x.SubCategoryId == spareAccessorTransp.SubCategoryId).ToListAsync();
                foreach (var item in images)
                {
                    _fileService.DeleteFile(item.ImageName);
                }
                _context.Pictures.RemoveRange(images);
                await _context.SaveChangesAsync();
                foreach (var item in spareAccessorTransp.Images)
                {
                    var imageName = _fileService.CreateFile(item);
                    if (imageName.StatusCode != (int)System.Net.HttpStatusCode.OK)
                    {
                        continue;
                    }
                    var image = new Picture
                    {
                        ImageName = imageName.Data!,
                        ProductType = ProductType.SpareAccessorTransp,
                        ProductId = spareAccessorTransp.Id,
                        SubCategoryId = spareAccessorTransp.SubCategoryId
                    };
                    await _context.Pictures.AddAsync(image);
                    await _context.SaveChangesAsync();

                }
            }
            return new Response<string>(System.Net.HttpStatusCode.OK, $"{spareAccessorTransp.Model} was updated successfully");
        }
        return new Response<string>(System.Net.HttpStatusCode.NotFound, $"{spareAccessorTransp.Model} not found");
    }

    public async Task<Response<string>> DeleteSpareAccessorTransp(int spareAccessorTranspId, string currentUserId, bool isPrivileged)
    {
        var find = await _context.SpareAccessorTransps.FirstOrDefaultAsync(x => x.Id == spareAccessorTranspId);
        if (find != null)
        {
            if (!isPrivileged && find.OwnerId != currentUserId)
            {
                return new Response<string>(System.Net.HttpStatusCode.Forbidden, "You do not have access to this listing");
            }
            var images = await _context.Pictures.Where(x => x.ProductType == ProductType.SpareAccessorTransp && x.ProductId == find.Id && x.SubCategoryId == find.SubCategoryId).ToListAsync();
            foreach (var item in images)
            {
                _fileService.DeleteFile(item.ImageName);
            }
            _context.Pictures.RemoveRange(images);
            _context.SpareAccessorTransps.Remove(find);
            await _context.SaveChangesAsync();
            return new Response<string>(System.Net.HttpStatusCode.OK, $"{find.Model} deleted successfully");
        }
        return new Response<string>(System.Net.HttpStatusCode.NotFound, "Spare or Accessor not found");
    }
}
