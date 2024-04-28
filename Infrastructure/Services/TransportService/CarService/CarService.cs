using System.ComponentModel;
using System.Net;
using AutoMapper;
using Domain.DTOs.PictureDTO;
using Domain.DTOs.TransportDTOs.CarsDTOs;
using Domain.Entities;
using Domain.Entities.Transport;
using Domain.Filters.TransportFilters.CarsFilter;
using Domain.Responses;
using Infrastructure.Data;
using Infrastructure.Services.FileService;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.TransportService.CarService;

public class CarService : ICarService
{
    private readonly IMapper _mapper;
    private readonly DataContext _context;
    private readonly IFileService _fileService;
    public CarService(IMapper mapper, DataContext context, IFileService fileService)
    {
        _context = context;
        _fileService = fileService;
        _mapper = mapper;
    }


    public async Task<PagedResponse<List<GetCarDTO>>> GetCar(GetCarFilter filter)
    {
        var query = _context.Cars.AsQueryable();
        if (filter.Model != null)
        {
            query = query.Where(x => x.Model.ToLower().Contains(filter.Model.ToLower()));
        }
        var mapped = await (from c in query
                            select new GetCarDTO
                            {
                                Id = c.Id,
                                Price = c.Price,
                                DiscountPrice = c.DiscountPrice,
                                YearOfIssue = c.YearOfIssue,
                                Brand = c.Brand,
                                Engine = c.Engine,
                                Body = c.Body,
                                Gearbox = c.Gearbox,
                                DriverUnit = c.DriverUnit,
                                EngineCapacity = c.EngineCapacity,
                                Mileage = c.Mileage,
                                ManufacturerState = c.ManufacturerState,
                                Model = c.Model,
                                FuelPer100km = c.FuelPer100km,
                                NumberOfSeats = c.NumberOfSeats,
                                Condition = c.Condition,
                                AccelerTo100km = c.AccelerTo100km,
                                TrunkVolume = c.TrunkVolume,
                                Clearance = c.Clearance,
                                SteeringWheel = c.SteeringWheel,
                                Color = c.Color,
                                PowerSteering = c.PowerSteering,
                                InteriorColor = c.InteriorColor,
                                SettingsMemory = c.SettingsMemory,
                                MultimediaAndNavigation = c.MultimediaAndNavigation,
                                ClimateControl = c.ClimateControl,
                                DrivingAssistance = c.DrivingAssistance,
                                AntiTheftSystem = c.AntiTheftSystem,
                                Airbags = c.Airbags,
                                Heating = c.Heating,
                                TiresAndWheels = c.TiresAndWheels,
                                Headlights = c.Headlights,
                                AudioSystems = c.AudioSystems,
                                ElectricLifts = c.ElectricLifts,
                                ElectricDrive = c.ElectricDrive,
                                ActiveSafety = c.ActiveSafety,
                                Images = _context.Pictures
                                    .Where(p => p.ProductId == c.Id && p.SubCategoryId == c.SubCategoryId)
                                    .Select(s => new PictureDto { Id = s.Id, ImageName = s.ImageName })
                                    .ToList()
                            }).Skip((filter.PageNumber - 1) * filter.PageSize)
                            .Take(filter.PageSize).ToListAsync();
        var totalCount = await query.CountAsync();
        return new PagedResponse<List<GetCarDTO>>(mapped, filter.PageNumber, filter.PageSize, totalCount);
    }

    public async Task<Response<GetCarDTO>> GetCarById(int CarId)
    {
        var query = _context.Cars.AsQueryable();
        query = query.Where(c => c.Id == CarId);
        if (query == null)
        {
            return new Response<GetCarDTO>(System.Net.HttpStatusCode.NotFound, "Car not found");
        }
        var mapped = await (from c in query
                            select new GetCarDTO
                            {
                                Id = c.Id,
                                Price = c.Price,
                                DiscountPrice = c.DiscountPrice,
                                YearOfIssue = c.YearOfIssue,
                                Brand = c.Brand,
                                Engine = c.Engine,
                                Body = c.Body,
                                Gearbox = c.Gearbox,
                                DriverUnit = c.DriverUnit,
                                EngineCapacity = c.EngineCapacity,
                                Mileage = c.Mileage,
                                ManufacturerState = c.ManufacturerState,
                                Model = c.Model,
                                FuelPer100km = c.FuelPer100km,
                                NumberOfSeats = c.NumberOfSeats,
                                Condition = c.Condition,
                                AccelerTo100km = c.AccelerTo100km,
                                TrunkVolume = c.TrunkVolume,
                                Clearance = c.Clearance,
                                SteeringWheel = c.SteeringWheel,
                                Color = c.Color,
                                PowerSteering = c.PowerSteering,
                                InteriorColor = c.InteriorColor,
                                SettingsMemory = c.SettingsMemory,
                                MultimediaAndNavigation = c.MultimediaAndNavigation,
                                ClimateControl = c.ClimateControl,
                                DrivingAssistance = c.DrivingAssistance,
                                AntiTheftSystem = c.AntiTheftSystem,
                                Airbags = c.Airbags,
                                Heating = c.Heating,
                                TiresAndWheels = c.TiresAndWheels,
                                Headlights = c.Headlights,
                                AudioSystems = c.AudioSystems,
                                ElectricLifts = c.ElectricLifts,
                                ElectricDrive = c.ElectricDrive,
                                ActiveSafety = c.ActiveSafety,
                                Images = _context.Pictures
                                                        .Where(p => p.ProductId == c.Id && p.SubCategoryId == c.SubCategoryId)
                                                        .Select(s => new PictureDto { Id = s.Id, ImageName = s.ImageName })
                                                        .ToList()

                            }).FirstOrDefaultAsync();
        return new Response<GetCarDTO>(mapped);
    }

    public async Task<Response<string>> AddCar(AddCarDTO car)
    {
        if (car == null)
        {
            return new Response<string>(HttpStatusCode.NotFound, "Please fill the parameters");
        }
        var mapped = _mapper.Map<Car>(car);
        await _context.Cars.AddAsync(mapped);
        await _context.SaveChangesAsync();
        foreach (var item in car.Images)
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
        return new Response<string>(HttpStatusCode.OK, $"{car.Model} added successfully");
    }

    public async Task<Response<string>> UpdateCar(AddCarDTO car)
    {
        var find = await _context.Cars.FirstOrDefaultAsync(x => x.Id == car.Id);
        if (find != null)
        {
            var mapped = _mapper.Map<Car>(car);
            await _context.Cars.AddAsync(mapped);
            await _context.SaveChangesAsync();
            if (car.Images != null)
            {
                var images = await _context.Pictures.
                Where(x => x.ProductId == car.Id && x.SubCategoryId == car.SubCategoryId).
                ToListAsync();
                foreach (var item in images)
                {
                    _fileService.DeleteFile(item.ImageName);
                }
                _context.Pictures.RemoveRange(images);
                await _context.SaveChangesAsync();
                foreach (var item in car.Images)
                {
                    var imageName = _fileService.CreateFile(item);
                    var image = new Picture
                    {
                        ImageName = imageName.Data!,
                        ProductId = car.Id,
                        SubCategoryId = car.SubCategoryId
                    };
                    await _context.AddAsync(image);
                    await _context.SaveChangesAsync();
                }
            }
            return new Response<string>(HttpStatusCode.OK, $"{car.Model} was updated successfully");
        }
        return new Response<string>(HttpStatusCode.NotFound, $"{car.Model}  not found");
    }

    public async Task<Response<string>> DeleteCar(int carId)
    {
        var find = await _context.Cars.FirstOrDefaultAsync(x => x.Id == carId);
        if (find != null)
        {
            var images = await _context.Pictures.
            Where(x => x.ProductId == find.Id && x.SubCategoryId == find.SubCategoryId).
            ToListAsync();
            foreach (var item in images)
            {
                _fileService.DeleteFile(item.ImageName);
            }
            _context.Pictures.RemoveRange(images);
            _context.Cars.Remove(find);
            await _context.SaveChangesAsync();
            return new Response<string>(HttpStatusCode.OK, $"{find.Model}  deleted successfully");
        }
        return new Response<string>(HttpStatusCode.NotFound, "Car not found");
    }
}
