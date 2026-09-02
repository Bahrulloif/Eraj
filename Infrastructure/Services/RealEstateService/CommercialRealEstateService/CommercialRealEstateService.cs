using AutoMapper;
using Domain.DTOs.PictureDTO;
using Domain.DTOs.RealEstateDTOs.CommercialRealEstateDTOs;
using Domain.Entities;
using Domain.Entities.RealEstate;
using Domain.Filters.RealEstateFilters.CommercialRealEstateFilter;
using Domain.Responses;
using Infrastructure.Data;
using Infrastructure.Services.FileService;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.RealEstateService.CommercialRealEstateService;

public class CommercialRealEstateService : ICommercialRealEstateService
{
    private readonly IMapper _mapper;
    private readonly DataContext _context;
    private readonly IFileService _fileService;

    public CommercialRealEstateService(IMapper mapper, DataContext context, IFileService fileService)
    {
        _mapper = mapper;
        _context = context;
        _fileService = fileService;
    }

    public async Task<PagedResponse<List<GetCommercialRealEstateDTO>>> GetCommercialRealEstate(GetCommercialRealEstateFilter filter)
    {
        var query = _context.CommercialRealEstates.AsQueryable();
        var mapped = await (from c in query
                            select new GetCommercialRealEstateDTO
                            {
                                Id = c.Id,
                                SubCategoryId = c.SubCategoryId,
                                Price = c.Price,
                                Area = c.Area,
                                BuildingType = c.BuildingType,
                                Floor = c.Floor,
                                Images = _context.Pictures
                                    .Where(p => p.ProductId == c.Id && p.SubCategoryId == c.SubCategoryId)
                                    .Select(s => new PictureDto { Id = s.Id, ImageName = s.ImageName })
                                    .ToList()
                            }).Skip((filter.PageNumber - 1) * filter.PageSize)
                            .Take(filter.PageSize).ToListAsync();
        var totalRecord = await query.CountAsync();
        return new PagedResponse<List<GetCommercialRealEstateDTO>>(mapped, filter.PageNumber, filter.PageSize, totalRecord);
    }

    public async Task<Response<GetCommercialRealEstateDTO>> GetCommercialRealEstateById(int commercialRealEstateId)
    {
        var mapped = await (from c in _context.CommercialRealEstates.Where(x => x.Id == commercialRealEstateId)
                            select new GetCommercialRealEstateDTO
                            {
                                Id = c.Id,
                                SubCategoryId = c.SubCategoryId,
                                Price = c.Price,
                                Area = c.Area,
                                BuildingType = c.BuildingType,
                                Floor = c.Floor,
                                Images = _context.Pictures
                                    .Where(p => p.ProductId == c.Id && p.SubCategoryId == c.SubCategoryId)
                                    .Select(s => new PictureDto { Id = s.Id, ImageName = s.ImageName })
                                    .ToList()
                            }).FirstOrDefaultAsync();
        if (mapped == null)
        {
            return new Response<GetCommercialRealEstateDTO>(System.Net.HttpStatusCode.NotFound, "CommercialRealEstate not found");
        }
        return new Response<GetCommercialRealEstateDTO>(mapped);
    }

    public async Task<Response<string>> AddCommercialRealEstate(AddCommercialRealEstateDTO commercialRealEstate, string currentUserId)
    {
        var mapped = _mapper.Map<CommercialRealEstate>(commercialRealEstate);
        mapped.OwnerId = currentUserId;
        await _context.CommercialRealEstates.AddAsync(mapped);
        await _context.SaveChangesAsync();
        foreach (var item in commercialRealEstate.Images)
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
        return new Response<string>(System.Net.HttpStatusCode.OK, "CommercialRealEstate added successfully");
    }

    public async Task<Response<string>> UpdateCommercialRealEstate(AddCommercialRealEstateDTO commercialRealEstate, string currentUserId, bool isPrivileged)
    {
        var find = await _context.CommercialRealEstates.FirstOrDefaultAsync(x => x.Id == commercialRealEstate.Id);
        if (find == null)
        {
            return new Response<string>(System.Net.HttpStatusCode.NotFound, "CommercialRealEstate not found");
        }
        if (!isPrivileged && find.OwnerId != currentUserId)
        {
            return new Response<string>(System.Net.HttpStatusCode.Forbidden, "You do not have access to this listing");
        }
        _mapper.Map(commercialRealEstate, find);
        _context.CommercialRealEstates.Update(find);
        await _context.SaveChangesAsync();
        if (commercialRealEstate.Images != null)
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
            foreach (var item in commercialRealEstate.Images)
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
        return new Response<string>(System.Net.HttpStatusCode.OK, "CommercialRealEstate was updated successfully");
    }

    public async Task<Response<string>> DeleteCommercialRealEstate(int commercialRealEstateId, string currentUserId, bool isPrivileged)
    {
        var find = await _context.CommercialRealEstates.FirstOrDefaultAsync(x => x.Id == commercialRealEstateId);
        if (find == null)
        {
            return new Response<string>(System.Net.HttpStatusCode.NotFound, "CommercialRealEstate not found");
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
        _context.CommercialRealEstates.Remove(find);
        await _context.SaveChangesAsync();
        return new Response<string>(System.Net.HttpStatusCode.OK, "CommercialRealEstate deleted successfully");
    }
}
