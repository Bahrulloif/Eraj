using AutoMapper;
using Domain.DTOs.PictureDTO;
using Domain.DTOs.TransportDTOs.MotorbikeDTOs;
using Domain.Entities;
using Domain.Entities.Transport;
using Domain.Filters.TransportFilters.GetMotorbikeFilter;
using Domain.Responses;
using Infrastructure.Data;
using Infrastructure.Services.FileService;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.TransportService.MotorbikeService;

public class MotorbikeService : IMotorbikeService
{

    private readonly IMapper _mapper;
    private readonly DataContext _context;
    private readonly IFileService _fileService;

    public MotorbikeService(IMapper mapper, DataContext context, IFileService fileService)
    {
        _context = context;
        _fileService = fileService;
        _mapper = mapper;
    }

    public async Task<PagedResponse<List<GetMotorbikeDTO>>> GetMotorbike(GetMotorbikeFilter filter)
    {
        var query = _context.Motorbikes.AsQueryable();
        if (filter.Model != null)
        {
            query = query.Where(x => x.Model.ToLower().Contains(filter.Model.ToLower()));
        }
        var mapped = await (from m in query
                            select new GetMotorbikeDTO
                            {
                                Id = m.Id,
                                Model = m.Model,
                                Region = m.Region,
                                Price = m.Price,
                                DiscountPrice = m.DiscountPrice,
                                Brand = m.Brand,
                                YearOfIssue = m.YearOfIssue,
                                ManufacturerCountry = m.ManufacturerCountry,
                                EngineType = m.EngineType,
                                EngineCapacity = m.EngineCapacity,
                                Power = m.Power,
                                FuelSupply = m.FuelSupply,
                                NumberOfCycles = m.NumberOfCycles,
                                GearBox = m.GearBox,
                                Mileage = m.Mileage,
                                Passengers = m.Passengers,
                                Images = _context.Pictures
                                          .Where(p => p.ProductId == m.Id && p.SubCategoryId == m.SubCategoryId)
                                          .Select(s => new PictureDto { Id = s.Id, ImageName = s.ImageName })
                                          .ToList()
                            }).Skip((filter.PageNumber - 1) * filter.PageSize)
                            .Take(filter.PageSize).ToListAsync();
        var totalCount = await query.CountAsync();
        return new PagedResponse<List<GetMotorbikeDTO>>(mapped, filter.PageNumber, filter.PageSize, totalCount);
    }


    public async Task<Response<GetMotorbikeDTO>> GetMotorbikeById(int motorbikeId)
    {
        var query = _context.Motorbikes.AsQueryable();
        var find = query.Where(x => x.Id == motorbikeId).FirstOrDefaultAsync();
        if (find != null)
        {
            var mapped = await (from m in query
                                select new GetMotorbikeDTO
                                {
                                    Id = m.Id,
                                    Model = m.Model,
                                    Region = m.Region,
                                    Price = m.Price,
                                    DiscountPrice = m.DiscountPrice,
                                    Brand = m.Brand,
                                    YearOfIssue = m.YearOfIssue,
                                    ManufacturerCountry = m.ManufacturerCountry,
                                    EngineType = m.EngineType,
                                    EngineCapacity = m.EngineCapacity,
                                    Power = m.Power,
                                    FuelSupply = m.FuelSupply,
                                    NumberOfCycles = m.NumberOfCycles,
                                    GearBox = m.GearBox,
                                    Mileage = m.Mileage,
                                    Passengers = m.Passengers,
                                    Images = _context.Pictures
                                                              .Where(p => p.ProductId == m.Id && p.SubCategoryId == m.SubCategoryId)
                                                              .Select(s => new PictureDto { Id = s.Id, ImageName = s.ImageName })
                                                              .ToList()
                                }).FirstOrDefaultAsync();
            return new Response<GetMotorbikeDTO>(mapped);
        }
        return new Response<GetMotorbikeDTO>(System.Net.HttpStatusCode.NotFound, "Motorbike not found");
    }

    public async Task<Response<string>> AddMotorbike(AddMotorbikeDTO motorbike)
    {
        var mapped = _mapper.Map<Motorbike>(motorbike);
        await _context.Motorbikes.AddAsync(mapped);
        foreach (var item in motorbike.Images)
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
        return new Response<string>(System.Net.HttpStatusCode.OK, "Motorbike added successfully");
    }

    public async Task<Response<string>> UpdateMotorbike(AddMotorbikeDTO motorbike)
    {
        var find = await _context.Motorbikes.FirstOrDefaultAsync(x => x.Id == motorbike.Id);
        if (find == null)
        {
            return new Response<string>(System.Net.HttpStatusCode.NotFound, "Motorbike not found");
        }
        var mapped = _mapper.Map<Motorbike>(motorbike);
        await _context.Motorbikes.AddAsync(mapped);
        if (motorbike.Images != null)
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
            foreach (var item in motorbike.Images)
            {
                var imageName = _fileService.CreateFile(item);
                var image = new Picture
                {
                    ImageName = imageName.Data!,
                    ProductId = motorbike.Id,
                    SubCategoryId = motorbike.SubCategoryId
                };
                await _context.Pictures.AddAsync(image);
                await _context.SaveChangesAsync();
            }
        }
        return new Response<string>(System.Net.HttpStatusCode.OK, "The Motorbike is updated successfully");
    }

    public async Task<Response<string>> DeleteMotorbike(int motorbikeId)
    {
        var find = await _context.Motorbikes.FirstOrDefaultAsync(x => x.Id == motorbikeId);
        if (find == null)
        {
            return new Response<string>(System.Net.HttpStatusCode.NotFound, "Motorbike not found");
        }
        var images = await _context.Pictures.
        Where(x => x.ProductId == find.Id && x.SubCategoryId == find.SubCategoryId).
        ToListAsync();
        foreach (var item in images)
        {
            _fileService.DeleteFile(item.ImageName);
        }
        _context.Pictures.RemoveRange(images);
        _context.Motorbikes.Remove(find);
        await _context.SaveChangesAsync();
        return new Response<string>(System.Net.HttpStatusCode.OK, "Motorbike deleted successfully");
    }
}
