using AutoMapper;
using Domain.DTOs.PictureDTO;
using Domain.DTOs.TransportDTOs.SpareAccessorTranspDTOs;
using Domain.Entities;
using Domain.Entities.Transport;
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
        var mapped = await (from a in query
                            select new GetSpareAccessorTranspDTO
                            {
                                Id = a.Id,
                                Model = a.Model,
                                Description = a.Description,
                                Price = a.Price,
                                DiscountPrice = a.DiscountPrice,
                                Images = _context.Pictures
                                    .Where(p => p.ProductId == a.Id && p.SubCategoryId == a.SubCategoryId)
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
                                Model = a.Model,
                                Description = a.Description,
                                Price = a.Price,
                                DiscountPrice = a.DiscountPrice,
                                Images = _context.Pictures
                                    .Where(p => p.ProductId == a.Id && p.SubCategoryId == a.SubCategoryId)
                                    .Select(s => new PictureDto { Id = s.Id, ImageName = s.ImageName })
                                    .ToList()
                            }).FirstOrDefaultAsync();
        return new Response<GetSpareAccessorTranspDTO>(mapped);
    }

    public async Task<Response<string>> AddSpareAccessorTransp(AddSpareAccessorTranspDTO spareAccessorTransp)
    {
        var mapped = _mapper.Map<SpareAccessorTransp>(spareAccessorTransp);
        await _context.SpareAccessorTransps.AddAsync(mapped);
        await _context.SaveChangesAsync();
        if (spareAccessorTransp.Images != null)
        {
            foreach (var item in spareAccessorTransp.Images)
            {
                var imageName = _fileService.CreateFile(item);
                var image = new Picture
                {
                    ImageName = imageName.Data!,
                    ProductId = spareAccessorTransp.Id,
                    SubCategoryId = spareAccessorTransp.SubCategoryId
                };
                await _context.Pictures.AddAsync(image);
                await _context.SaveChangesAsync();
            }
        }
        return new Response<string>(System.Net.HttpStatusCode.OK, $"{spareAccessorTransp.Model} added successfully");
    }

    public async Task<Response<string>> UpdateSpareAccessorTransp(AddSpareAccessorTranspDTO spareAccessorTransp)
    {
        var find = await _context.SpareAccessorTransps.FirstOrDefaultAsync(x => x.Id == spareAccessorTransp.Id);
        if (find != null)
        {
            var mapped = _mapper.Map<SpareAccessorTransp>(spareAccessorTransp);
            await _context.SpareAccessorTransps.AddAsync(mapped);
            await _context.SaveChangesAsync();
            if (spareAccessorTransp.Images != null)
            {
                var images = await _context.Pictures.Where(x => x.ProductId == spareAccessorTransp.Id && x.SubCategoryId == spareAccessorTransp.SubCategoryId).ToListAsync();
                foreach (var item in images)
                {
                    _fileService.DeleteFile(item.ImageName);
                }
                _context.Pictures.RemoveRange(images);
                await _context.SaveChangesAsync();
                foreach (var item in spareAccessorTransp.Images)
                {
                    var imageName = _fileService.CreateFile(item);
                    var image = new Picture
                    {
                        ImageName = imageName.Data!,
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

    public async Task<Response<string>> DeleteSpareAccessorTransp(int spareAccessorTranspId)
    {
        var find = await _context.SpareAccessorTransps.FirstOrDefaultAsync(x => x.Id == spareAccessorTranspId);
        if (find != null)
        {
            var images = await _context.Pictures.Where(x => x.ProductId == spareAccessorTranspId && x.SubCategoryId == spareAccessorTranspId).ToListAsync();
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
