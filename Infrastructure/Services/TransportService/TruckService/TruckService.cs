using AutoMapper;
using Domain.DTOs.PictureDTO;
using Domain.DTOs.TransportDTOs.TruckDTOs;
using Domain.Entities;
using Domain.Entities.Transport;
using Domain.Filters.TransportFilter.TruckFilters;
using Domain.Responses;
using Infrastructure.Data;
using Infrastructure.Services.FileService;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.TransportService.TruckService;

public class TruckService : ITruckService
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;
    private readonly IFileService _fileService;

    public TruckService(IFileService fileService, DataContext context, IMapper mapper)
    {
        _fileService = fileService;
        _context = context;
        _mapper = mapper;
    }

    public async Task<PagedResponse<List<GetTruckDTO>>> GetTruck(TruckFilter filter)
    {
        var query = _context.Trucks.AsQueryable();
        if (filter.Model != null)
        {
            query = query.Where(x => x.Model.ToLower().Contains(filter.Model.ToLower()));
        }
        var mapped = await (from t in query
                            select new GetTruckDTO
                            {
                                Id = t.Id,
                                SubCategoryId = t.SubCategoryId,
                                Price = t.Price,
                                PriceDiscount = t.PriceDiscount,
                                Brand = t.Brand,
                                Model = t.Model,
                                YearOfIssue = t.YearOfIssue,
                                BodyType = t.BodyType,
                                Power = t.Power,
                                EngineType = t.EngineType,
                                EngineCapacity = t.EngineCapacity,
                                EnvironmentalClass = t.EnvironmentalClass,
                                Transmission = t.Transmission,
                                WheelFormula = t.WheelFormula,
                                LoadCapacity = t.LoadCapacity,
                                PermittedMaximumWeight = t.PermittedMaximumWeight,
                                Mileage = t.Mileage,
                                Images = _context.Pictures.
                                Where(x => x.ProductId == t.Id && x.SubCategoryId == t.SubCategoryId).
                                Select(c => new PictureDto { Id = c.Id, ImageName = c.ImageName }).
                                ToList()
                            }).Skip((filter.PageNumber - 1) * filter.PageSize).
                            Take(filter.PageSize).ToListAsync();
        var totalRecord = await query.CountAsync();
        return new PagedResponse<List<GetTruckDTO>>(mapped, filter.PageNumber, filter.PageSize, totalRecord);
    }

    public async Task<Response<GetTruckDTO>> GetTruckById(int truckId)
    {
        var query = _context.Trucks.AsQueryable();
        query = query.Where(x => x.Id == truckId);
        if (query == null)
        {
            return new Response<GetTruckDTO>(System.Net.HttpStatusCode.NotFound, "Truck not found");
        }
        var mapped = await (from t in query
                            select new GetTruckDTO
                            {
                                Id = t.Id,
                                SubCategoryId = t.SubCategoryId,
                                Price = t.Price,
                                PriceDiscount = t.PriceDiscount,
                                Brand = t.Brand,
                                Model = t.Model,
                                YearOfIssue = t.YearOfIssue,
                                BodyType = t.BodyType,
                                Power = t.Power,
                                EngineType = t.EngineType,
                                EngineCapacity = t.EngineCapacity,
                                EnvironmentalClass = t.EnvironmentalClass,
                                Transmission = t.Transmission,
                                WheelFormula = t.WheelFormula,
                                LoadCapacity = t.LoadCapacity,
                                PermittedMaximumWeight = t.PermittedMaximumWeight,
                                Mileage = t.Mileage,
                                Images = _context.Pictures.
                                Where(x => x.ProductId == t.Id && x.SubCategoryId == t.SubCategoryId).
                                Select(c => new PictureDto { Id = c.Id, ImageName = c.ImageName }).
                                ToList()
                            }).FirstOrDefaultAsync();
        return new Response<GetTruckDTO>(mapped);
    }

    public async Task<Response<string>> AddTruck(AddTruckDTO truck)
    {
        var mapped = _mapper.Map<Truck>(truck);
        await _context.Trucks.AddRangeAsync(mapped);
        await _context.SaveChangesAsync();
        foreach (var item in truck.Images)
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
        return new Response<string>(System.Net.HttpStatusCode.OK, $"{truck.Model} added successfully");
    }

    public async Task<Response<string>> UpdateTruck(AddTruckDTO truck)
    {
        var mapped = _mapper.Map<Truck>(truck);
        await _context.Trucks.AddAsync(mapped);
        await _context.SaveChangesAsync();
        if (truck.Images != null)
        {
            var images = await _context.Pictures.
            Where(x => x.ProductId == truck.Id && x.SubCategoryId == truck.SubCategoryId).
            ToListAsync();
            foreach (var item in images)
            {
                _fileService.DeleteFile(item.ImageName);
            }
            _context.Pictures.RemoveRange(images);
            await _context.SaveChangesAsync();

            foreach (var item in truck.Images)
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
        return new Response<string>(System.Net.HttpStatusCode.OK, $"{truck.Model} was updated successfully");
    }

    public async Task<Response<string>> DeleteTruck(int truckId)
    {
        var find = await _context.Trucks.FirstOrDefaultAsync(x => x.Id == truckId);
        if (find == null)
        {
            return new Response<string>(System.Net.HttpStatusCode.NotFound, "Truck not found");
        }
        var images = await _context.Pictures.
        Where(x => x.ProductId == find.Id && x.SubCategoryId == find.SubCategoryId).
        ToListAsync();
        foreach (var item in images)
        {
            _fileService.DeleteFile(item.ImageName);
        }
        _context.Pictures.RemoveRange(images);
        return new Response<string>(System.Net.HttpStatusCode.OK, $"{find.Model} deleted successfully");
    }
}