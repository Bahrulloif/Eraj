using AutoMapper;
using Domain.DTOs.KomTechDTOs.SpareAccessorKompDTOs;
using Domain.DTOs.PictureDTO;
using Domain.DTOs.TransportDTOs.SpareAccessorKompDTOs;
using Domain.Entities;
using Domain.Entities.KompTech;
using Domain.Filters.KompTechFilters.SpareAccessorKompFilter;
using Domain.Responses;
using Infrastructure.Data;
using Infrastructure.Services.FileService;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.KompTechService.SpareAccessorKompService;

public class SpareAccessorKompService : ISpareAccessorKompService

{
    private readonly IMapper _mapper;
    private readonly DataContext _context;
    private readonly IFileService _fileService;

    public SpareAccessorKompService(IMapper mapper, DataContext context, IFileService fileService)
    {
        _mapper = mapper;
        _context = context;
        _fileService = fileService;
    }

    public async Task<PagedResponse<List<GetSpareAccessorKompDTO>>> GetSpareAccessorKomp(GetSpareAccessorKompFilter filter)
    {
        var query = _context.SpareAccessorKomps.AsQueryable();
        if (filter.Name != null)
        {
            query = query.Where(x => x.Model.ToLower().Contains(filter.Name.ToLower()));
        }
        var mapped = await (from a in query
                            select new GetSpareAccessorKompDTO
                            {
                                Id = a.Id,
                                Model = a.Model,
                                Description = a.Description,
                                DiscountPrice = a.DiscountPrice,
                                Price = a.Price,
                                Images = _context.Pictures
                                .Where(x => x.ProductId == a.Id && x.SubCategoryId == a.SubCategoryId)
                                .Select(s => new PictureDto { Id = s.Id, ImageName = s.ImageName })
                                .ToList()
                            })
                                .Skip((filter.PageNumber - 1) * filter.PageSize)
                                .Take(filter.PageSize).ToListAsync();
        var totalRecord = query.Count();
        return new PagedResponse<List<GetSpareAccessorKompDTO>>(mapped, filter.PageNumber, filter.PageSize, totalRecord);
    }

    public async Task<Response<GetSpareAccessorKompDTO>> GetSpareAccessorKompById(int spareAccessorKompId)
    {
        var query = _context.SpareAccessorKomps.AsQueryable();
        query = query.Where(x => x.Id == spareAccessorKompId);
        var mapped = await (from a in query
                            select new GetSpareAccessorKompDTO
                            {
                                Id = a.Id,
                                SubCategoryId = a.SubCategoryId,
                                Model = a.Model,
                                Description = a.Description,
                                DiscountPrice = a.DiscountPrice,
                                Price = a.Price,
                                Images = _context.Pictures
                                .Where(x => x.ProductId == a.Id && x.SubCategoryId == a.SubCategoryId)
                                .Select(s => new PictureDto { Id = s.Id, ImageName = s.ImageName })
                                .ToList()
                            }).FirstOrDefaultAsync();
        return new Response<GetSpareAccessorKompDTO>(mapped);
    }


    public async Task<Response<string>> AddSpareAccessorKomp(AddSpareAccessorKompDTO spareAccessorKomp)
    {
        var mapped = _mapper.Map<SpareAccessorKomp>(spareAccessorKomp);
        await _context.SpareAccessorKomps.AddAsync(mapped);
        await _context.SaveChangesAsync();
        if (spareAccessorKomp.Images != null)
        {
            foreach (var item in spareAccessorKomp.Images)
            {
                var imageName = _fileService.CreateFile(item);
                var image = new Picture
                {
                    ImageName = imageName.Data!,
                    ProductId = spareAccessorKomp.Id,
                    SubCategoryId = spareAccessorKomp.SubCategoryId
                };
                await _context.Pictures.AddAsync(image);
                await _context.SaveChangesAsync();
            }
        }
        return new Response<string>(System.Net.HttpStatusCode.OK, $"{spareAccessorKomp.Model} added successfully");
    }

    public async Task<Response<string>> UpdateSpareAccessorKomp(AddSpareAccessorKompDTO spareAccessorKomp)
    {
        var find = await _context.SpareAccessorKomps.FirstOrDefaultAsync(x => x.Id == spareAccessorKomp.Id);
        if (find != null)
        {
            var mapped = _mapper.Map<SpareAccessorKomp>(spareAccessorKomp);
            await _context.SpareAccessorKomps.AddAsync(mapped);
            await _context.SaveChangesAsync();
            if (spareAccessorKomp.Images != null)
            {
                var images = await _context.Pictures
                .Where(x => x.ProductId == spareAccessorKomp.Id && x.SubCategoryId == spareAccessorKomp.SubCategoryId)
                .ToListAsync();
                foreach (var item in images)
                {
                    _fileService.DeleteFile(item.ImageName);
                }
                _context.Pictures.RemoveRange(images);
                await _context.SaveChangesAsync();
                foreach (var item in spareAccessorKomp.Images)
                {
                    var imageName = _fileService.CreateFile(item);
                    var image = new Picture
                    {
                        ImageName = imageName.Data!,
                        ProductId = spareAccessorKomp.Id,
                        SubCategoryId = spareAccessorKomp.SubCategoryId
                    };
                    await _context.Pictures.AddAsync(image);
                    await _context.SaveChangesAsync();
                }
            }
            return new Response<string>(System.Net.HttpStatusCode.OK, $"{spareAccessorKomp.Model} was update successfully");
        }
        return new Response<string>(System.Net.HttpStatusCode.NotFound, $"{spareAccessorKomp.Model} not found");
    }


    public async Task<Response<string>> DeleteSpareAccessorKomp(int spareAccessorKompId)
    {
        var find = await _context.SpareAccessorKomps.FirstOrDefaultAsync(x => x.Id == spareAccessorKompId);
        if (find != null)
        {
            var images = await _context.Pictures
            .Where(x => x.ProductId == spareAccessorKompId && x.SubCategoryId == spareAccessorKompId)
            .ToListAsync();
            foreach (var item in images)
            {
                _fileService.DeleteFile(item.ImageName);
            }
            _context.Pictures.RemoveRange(images);
            _context.SpareAccessorKomps.Remove(find);
            await _context.SaveChangesAsync();
            return new Response<string>(System.Net.HttpStatusCode.OK, $"{find.Model} deleted successfully");
        }
        return new Response<string>(System.Net.HttpStatusCode.NotFound, "Spare or Accessor not found");
    }
}
