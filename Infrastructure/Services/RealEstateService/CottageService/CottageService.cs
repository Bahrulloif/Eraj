using AutoMapper;
using Domain.DTOs.PictureDTO;
using Domain.DTOs.RealEstateDTOs.CottageDTOs;
using Domain.Entities;
using Domain.Entities.RealEstate;
using Domain.Filters.RealEstateFilters.CottageFilter;
using Domain.Responses;
using Infrastructure.Data;
using Infrastructure.Services.FileService;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.RealEstateService.CottageService;

public class CottageService : ICottageService
{
    private readonly IMapper _mapper;
    private readonly DataContext _context;
    private readonly IFileService _fileService;

    public CottageService(IMapper mapper, DataContext context, IFileService fileService)
    {
        _mapper = mapper;
        _context = context;
        _fileService = fileService;
    }

    public async Task<PagedResponse<List<GetCottageDTO>>> GetCottage(GetCottageFilter filter)
    {
        var query = _context.Cottages.AsQueryable();
        if (filter.NumberOfRooms != null)
        {
            query = query.Where(x => x.NumberOfRooms == filter.NumberOfRooms);
        }
        var mapped = await (from c in query
                            select new GetCottageDTO
                            {
                                Id = c.Id,
                                SubCategoryId = c.SubCategoryId,
                                TypeOfRealEstate = c.TypeOfRealEstate,
                                Price = c.Price,
                                PricePerM2 = c.PricePerM2,
                                HouseArea = c.HouseArea,
                                PlotArea = c.PlotArea,
                                Renovation = c.Renovation,
                                NumberOfRooms = c.NumberOfRooms,
                                WallMaterial = c.WallMaterial,
                                Parking = c.Parking,
                                Images = _context.Pictures
                                    .Where(p => p.ProductId == c.Id && p.SubCategoryId == c.SubCategoryId)
                                    .Select(s => new PictureDto { Id = s.Id, ImageName = s.ImageName })
                                    .ToList()
                            }).Skip((filter.PageNumber - 1) * filter.PageSize)
                            .Take(filter.PageSize).ToListAsync();
        var totalRecord = await query.CountAsync();
        return new PagedResponse<List<GetCottageDTO>>(mapped, filter.PageNumber, filter.PageSize, totalRecord);
    }

    public async Task<Response<GetCottageDTO>> GetCottageById(int cottageId)
    {
        var mapped = await (from c in _context.Cottages.Where(x => x.Id == cottageId)
                            select new GetCottageDTO
                            {
                                Id = c.Id,
                                SubCategoryId = c.SubCategoryId,
                                TypeOfRealEstate = c.TypeOfRealEstate,
                                Price = c.Price,
                                PricePerM2 = c.PricePerM2,
                                HouseArea = c.HouseArea,
                                PlotArea = c.PlotArea,
                                Renovation = c.Renovation,
                                NumberOfRooms = c.NumberOfRooms,
                                WallMaterial = c.WallMaterial,
                                Parking = c.Parking,
                                Images = _context.Pictures
                                    .Where(p => p.ProductId == c.Id && p.SubCategoryId == c.SubCategoryId)
                                    .Select(s => new PictureDto { Id = s.Id, ImageName = s.ImageName })
                                    .ToList()
                            }).FirstOrDefaultAsync();
        if (mapped == null)
        {
            return new Response<GetCottageDTO>(System.Net.HttpStatusCode.NotFound, "Cottage not found");
        }
        return new Response<GetCottageDTO>(mapped);
    }

    public async Task<Response<string>> AddCottage(AddCottageDTO cottage, string currentUserId)
    {
        var mapped = _mapper.Map<Cottage>(cottage);
        mapped.OwnerId = currentUserId;
        await _context.Cottages.AddAsync(mapped);
        await _context.SaveChangesAsync();
        foreach (var item in cottage.Images)
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
        return new Response<string>(System.Net.HttpStatusCode.OK, "Cottage added successfully");
    }

    public async Task<Response<string>> UpdateCottage(AddCottageDTO cottage, string currentUserId, bool isPrivileged)
    {
        var find = await _context.Cottages.FirstOrDefaultAsync(x => x.Id == cottage.Id);
        if (find == null)
        {
            return new Response<string>(System.Net.HttpStatusCode.NotFound, "Cottage not found");
        }
        if (!isPrivileged && find.OwnerId != currentUserId)
        {
            return new Response<string>(System.Net.HttpStatusCode.Forbidden, "You do not have access to this listing");
        }
        _mapper.Map(cottage, find);
        _context.Cottages.Update(find);
        await _context.SaveChangesAsync();
        if (cottage.Images != null)
        {
            var images = await _context.Pictures
                .Where(p => p.ProductId == find.Id && p.SubCategoryId == find.SubCategoryId)
                .ToListAsync();
            foreach (var item in images)
            {
                _fileService.DeleteFile(item.ImageName);
            }
            _context.Pictures.RemoveRange(images);
            await _context.SaveChangesAsync();
            foreach (var item in cottage.Images)
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
        return new Response<string>(System.Net.HttpStatusCode.OK, "Cottage was updated successfully");
    }

    public async Task<Response<string>> DeleteCottage(int cottageId, string currentUserId, bool isPrivileged)
    {
        var find = await _context.Cottages.FirstOrDefaultAsync(x => x.Id == cottageId);
        if (find == null)
        {
            return new Response<string>(System.Net.HttpStatusCode.NotFound, "Cottage not found");
        }
        if (!isPrivileged && find.OwnerId != currentUserId)
        {
            return new Response<string>(System.Net.HttpStatusCode.Forbidden, "You do not have access to this listing");
        }
        var images = await _context.Pictures
            .Where(p => p.ProductId == find.Id && p.SubCategoryId == find.SubCategoryId)
            .ToListAsync();
        foreach (var item in images)
        {
            _fileService.DeleteFile(item.ImageName);
        }
        _context.Pictures.RemoveRange(images);
        _context.Cottages.Remove(find);
        await _context.SaveChangesAsync();
        return new Response<string>(System.Net.HttpStatusCode.OK, "Cottage deleted successfully");
    }
}
