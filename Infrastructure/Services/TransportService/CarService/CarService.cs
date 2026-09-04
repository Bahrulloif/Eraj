using System.ComponentModel;
using System.Net;
using AutoMapper;
using Domain.DTOs.PictureDTO;
using Domain.DTOs.TransportDTOs.CarsDTOs;
using Domain.Entities;
using Domain.Entities.Transport;
using Domain.Enum;
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
        // Skip/Take needs a deterministic order to paginate correctly - without it SQL doesn't
        // guarantee row order, so results can drift or duplicate across pages.
        query = query.OrderBy(x => x.Id);
        var mapped = await (from c in query
                            select new GetCarDTO
                            {
                                Id = c.Id,
                                SubCategoryId = c.SubCategoryId,
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
                                    .Where(p => p.ProductType == ProductType.Car && p.ProductId == c.Id && p.SubCategoryId == c.SubCategoryId)
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
        var mapped = await (from c in query
                            select new GetCarDTO
                            {
                                Id = c.Id,
                                SubCategoryId = c.SubCategoryId,
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
                                                        .Where(p => p.ProductType == ProductType.Car && p.ProductId == c.Id && p.SubCategoryId == c.SubCategoryId)
                                                        .Select(s => new PictureDto { Id = s.Id, ImageName = s.ImageName })
                                                        .ToList()

                            }).FirstOrDefaultAsync();
        if (mapped == null)
        {
            return new Response<GetCarDTO>(System.Net.HttpStatusCode.NotFound, "Car not found");
        }
        return new Response<GetCarDTO>(mapped);
    }

    public async Task<Response<string>> AddCar(AddCarDTO car, string currentUserId)
    {
        if (car == null)
        {
            return new Response<string>(HttpStatusCode.NotFound, "Please fill the parameters");
        }
        var mapped = _mapper.Map<Car>(car);
        mapped.OwnerId = currentUserId;
        await _context.Cars.AddAsync(mapped);
        await _context.SaveChangesAsync();
        foreach (var item in car.Images)
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
                ProductType = ProductType.Car,
                ProductId = mapped.Id,
                SubCategoryId = mapped.SubCategoryId
            };
            await _context.Pictures.AddAsync(image);
            await _context.SaveChangesAsync();
        }
        return new Response<string>(HttpStatusCode.OK, $"{car.Model} added successfully");
    }

    public async Task<Response<string>> UpdateCar(AddCarDTO car, string currentUserId, bool isPrivileged)
    {
        var find = await _context.Cars.FirstOrDefaultAsync(x => x.Id == car.Id);
        if (find != null)
        {
            if (!isPrivileged && find.OwnerId != currentUserId)
            {
                return new Response<string>(HttpStatusCode.Forbidden, "You do not have access to this listing");
            }
            _mapper.Map(car, find);
            _context.Cars.Update(find);
            await _context.SaveChangesAsync();
            if (car.Images != null)
            {
                var images = await _context.Pictures.
                Where(x => x.ProductType == ProductType.Car && x.ProductId == car.Id && x.SubCategoryId == car.SubCategoryId).
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
                    if (imageName.StatusCode != (int)HttpStatusCode.OK)
                    {
                        continue;
                    }
                    var image = new Picture
                    {
                        ImageName = imageName.Data!,
                        ProductType = ProductType.Car,
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

    public async Task<Response<string>> DeleteCar(int carId, string currentUserId, bool isPrivileged)
    {
        var find = await _context.Cars.FirstOrDefaultAsync(x => x.Id == carId);
        if (find != null)
        {
            if (!isPrivileged && find.OwnerId != currentUserId)
            {
                return new Response<string>(HttpStatusCode.Forbidden, "You do not have access to this listing");
            }
            var images = await _context.Pictures.
            Where(x => x.ProductType == ProductType.Car && x.ProductId == find.Id && x.SubCategoryId == find.SubCategoryId).
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
