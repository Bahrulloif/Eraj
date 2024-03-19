using AutoMapper;
using Domain.DTOs.KomTechDTOs.SmartPhoneDTOs;
using Domain.DTOs.PictureDTO;
using Domain.Entities;
using Domain.Entities.KompTech;
using Domain.Filters.KompTechFilters.SmartPhoneFilters;
using Domain.Responses;
using Infrastructure.Data;
using Infrastructure.Services.FileService;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Infrastructure.Services.KompTechService.SmartPhoneService;

public class SmartPhoneService : ISmartPhoneService 
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;
    private readonly IFileService _fileService;
    public SmartPhoneService(DataContext context, IMapper mapper, IFileService fileService)
    {
        _context = context;
        _mapper = mapper;
        _fileService = fileService;
    }
    public async Task<PagedResponse<List<GetSmartPhoneDTO>>> GetSmartPhone(GetSmartPhoneFilter filter)
    {
        var query = _context.SmartPhones.AsQueryable();
        if (filter.Name != null)
        {
            query = query.Where(s => s.Model.ToLower().Contains(filter.Name.ToLower()));
        }
        var mapped = await (from s in query
                            select new GetSmartPhoneDTO
                            {
                                Id = s.Id,
                                Color = s.Color,
                                Core = s.Core,
                                Diagonal = s.Diagonal,
                                DiscountPrice = s.DiscountPrice,
                                Model = s.Model,
                                Price = s.Price,
                                RAM = s.RAM,
                                ROM = s.ROM,
                                Images = _context.Pictures.
                                    Where(p => p.ProductId == s.Id && p.SubCategoryId == s.SubCategoryId).
                                    Select(t => new PictureDto { Id = t.Id, ImageName = t.ImageName }).
                                    ToList()
                            }).
                                Skip((filter.PageNumber - 1) * filter.PageSize).
                                Take(filter.PageSize).ToListAsync();
        var totalCount = await query.CountAsync();
        return new PagedResponse<List<GetSmartPhoneDTO>>(mapped, filter.PageNumber, filter.PageSize, totalCount);
    }
    public async Task<Response<GetSmartPhoneDTO>> GetSmartPhoneById(int smartPhoneId)
    {
        var query = _context.SmartPhones.AsQueryable();
        query = query.Where(s => s.Id == smartPhoneId);
        // var find = await _context.SmartPhones.FirstOrDefaultAsync();
        if (query == null)
        {
            return new Response<GetSmartPhoneDTO>(HttpStatusCode.NotFound, "SmartPhone not found");
        }
        var mapped = await (from s in query
                            select new GetSmartPhoneDTO
                            {
                                Id = s.Id,
                                Color = s.Color,
                                Core = s.Core,
                                Diagonal = s.Diagonal,
                                DiscountPrice = s.DiscountPrice,
                                Model = s.Model,
                                Price = s.Price,
                                RAM = s.RAM,
                                ROM = s.ROM,
                                Images = _context.Pictures.
                                Where(p => p.Id == s.Id && p.SubCategoryId == s.SubCategoryId).
                                Select(x => new PictureDto { Id = x.Id, ImageName = x.ImageName }).
                                ToList()
                            }).
                            FirstOrDefaultAsync();
        return new Response<GetSmartPhoneDTO>(mapped);
    }
    public async Task<Response<string>> AddSmartPhone(AddSmartPhoneDTO smartPhone)
    {
        if (smartPhone == null)
        {
            return new Response<string>(HttpStatusCode.OK, "Please fill the SmartPhone");
        }
        var mapped = _mapper.Map<SmartPhone>(smartPhone);
        await _context.SmartPhones.AddAsync(mapped);
        await _context.SaveChangesAsync();
        foreach (var item in smartPhone.Images)
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
        return new Response<string>($"{mapped.Model} SmartPhone was added successfully");
    }
    public async Task<Response<string>> UpdateSmartPhone(AddSmartPhoneDTO smartPhone)
    {
        var find = await _context.SmartPhones.Where(s => s.Id == smartPhone.Id).AsNoTracking().FirstOrDefaultAsync();
        if (find != null)
        {
            var mapped = _mapper.Map<SmartPhone>(smartPhone);
            _context.SmartPhones.Update(mapped);
            if (smartPhone.Images != null)
            {
                var images = await _context.Pictures.Where(p => p.ProductId == smartPhone.Id && p.SubCategoryId == smartPhone.SubCategoryId).ToListAsync();
                foreach (var item in images)
                {
                    _fileService.DeleteFile(item.ImageName);
                    await _context.SaveChangesAsync();
                }
                _context.Pictures.RemoveRange(images);
                await _context.SaveChangesAsync();

                foreach (var item in smartPhone.Images)
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
            }
            await _context.SaveChangesAsync();
            return new Response<string>("SmartPhone updated successfully");
        }
        return new Response<string>(HttpStatusCode.NotFound, "SmartPhone not found");
    }
    public async Task<Response<string>> DeleteSmartPhone(int smartPhoneId)
    {
        var find = await _context.SmartPhones.FirstOrDefaultAsync(s => s.Id == smartPhoneId);
        if (find == null)
        {
            return new Response<string>(HttpStatusCode.NotFound, "SmartPhone not found");
        }
        var images = await _context.Pictures.Where(s => s.ProductId == find.Id && s.SubCategoryId == find.SubCategoryId).ToListAsync();
        foreach (var item in images)
        {
            _fileService.DeleteFile(item.ImageName);

        }
        _context.Pictures.RemoveRange(images);
        _context.SmartPhones.Remove(find);
        await _context.SaveChangesAsync();
        return new Response<string>("SmartPhone deleted successfully");
    }
}