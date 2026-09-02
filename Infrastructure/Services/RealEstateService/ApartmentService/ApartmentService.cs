using AutoMapper;
using Domain.DTOs.PictureDTO;
using Domain.DTOs.RealEstateDTOs.ApartmentDTOs;
using Domain.Entities;
using Domain.Entities.RealEstate;
using Domain.Filters.RealEstateFilters.ApartmentFilter;
using Domain.Responses;
using Infrastructure.Data;
using Infrastructure.Services.FileService;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.RealEstateService.ApartmentService;

public class ApartmentService : IApartmentService
{
    private readonly IMapper _mapper;
    private readonly DataContext _context;
    private readonly IFileService _fileService;

    public ApartmentService(IMapper mapper, DataContext context, IFileService fileService)
    {
        _mapper = mapper;
        _context = context;
        _fileService = fileService;
    }

    public async Task<PagedResponse<List<GetApartmentDTO>>> GetApartment(GetApartmentFilter filter)
    {
        var query = _context.Apartments.AsQueryable();
        if (filter.NumberOfRooms != null)
        {
            query = query.Where(x => x.NumberOfRooms == filter.NumberOfRooms);
        }
        var mapped = await (from a in query
                            select new GetApartmentDTO
                            {
                                Id = a.Id,
                                SubCategoryId = a.SubCategoryId,
                                NumberOfRooms = a.NumberOfRooms,
                                Price = a.Price,
                                PricePerM2 = a.PricePerM2,
                                TotalArea = a.TotalArea,
                                Floor = a.Floor,
                                Renovation = a.Renovation,
                                CeilingHeight = a.CeilingHeight,
                                YearOfHouseBuild = a.YearOfHouseBuild,
                                FloorsInTheHouse = a.FloorsInTheHouse,
                                Parking = a.Parking,
                                KitchenArea = a.KitchenArea,
                                IsNewBuilding = a.IsNewBuilding,
                                Images = _context.Pictures
                                    .Where(p => p.ProductId == a.Id && p.SubCategoryId == a.SubCategoryId)
                                    .Select(s => new PictureDto { Id = s.Id, ImageName = s.ImageName })
                                    .ToList()
                            }).Skip((filter.PageNumber - 1) * filter.PageSize)
                            .Take(filter.PageSize).ToListAsync();
        var totalRecord = await query.CountAsync();
        return new PagedResponse<List<GetApartmentDTO>>(mapped, filter.PageNumber, filter.PageSize, totalRecord);
    }

    public async Task<Response<GetApartmentDTO>> GetApartmentById(int apartmentId)
    {
        var mapped = await (from a in _context.Apartments.Where(x => x.Id == apartmentId)
                            select new GetApartmentDTO
                            {
                                Id = a.Id,
                                SubCategoryId = a.SubCategoryId,
                                NumberOfRooms = a.NumberOfRooms,
                                Price = a.Price,
                                PricePerM2 = a.PricePerM2,
                                TotalArea = a.TotalArea,
                                Floor = a.Floor,
                                Renovation = a.Renovation,
                                CeilingHeight = a.CeilingHeight,
                                YearOfHouseBuild = a.YearOfHouseBuild,
                                FloorsInTheHouse = a.FloorsInTheHouse,
                                Parking = a.Parking,
                                KitchenArea = a.KitchenArea,
                                IsNewBuilding = a.IsNewBuilding,
                                Images = _context.Pictures
                                    .Where(p => p.ProductId == a.Id && p.SubCategoryId == a.SubCategoryId)
                                    .Select(s => new PictureDto { Id = s.Id, ImageName = s.ImageName })
                                    .ToList()
                            }).FirstOrDefaultAsync();
        if (mapped == null)
        {
            return new Response<GetApartmentDTO>(System.Net.HttpStatusCode.NotFound, "Apartment not found");
        }
        return new Response<GetApartmentDTO>(mapped);
    }

    public async Task<Response<string>> AddApartment(AddApartmentDTO apartment, string currentUserId)
    {
        var mapped = _mapper.Map<Apartment>(apartment);
        mapped.OwnerId = currentUserId;
        await _context.Apartments.AddAsync(mapped);
        await _context.SaveChangesAsync();
        foreach (var item in apartment.Images)
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
        return new Response<string>(System.Net.HttpStatusCode.OK, "Apartment added successfully");
    }

    public async Task<Response<string>> UpdateApartment(AddApartmentDTO apartment, string currentUserId, bool isPrivileged)
    {
        var find = await _context.Apartments.FirstOrDefaultAsync(x => x.Id == apartment.Id);
        if (find == null)
        {
            return new Response<string>(System.Net.HttpStatusCode.NotFound, "Apartment not found");
        }
        if (!isPrivileged && find.OwnerId != currentUserId)
        {
            return new Response<string>(System.Net.HttpStatusCode.Forbidden, "You do not have access to this listing");
        }
        _mapper.Map(apartment, find);
        _context.Apartments.Update(find);
        await _context.SaveChangesAsync();
        if (apartment.Images != null)
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
            foreach (var item in apartment.Images)
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
        return new Response<string>(System.Net.HttpStatusCode.OK, "Apartment was updated successfully");
    }

    public async Task<Response<string>> DeleteApartment(int apartmentId, string currentUserId, bool isPrivileged)
    {
        var find = await _context.Apartments.FirstOrDefaultAsync(x => x.Id == apartmentId);
        if (find == null)
        {
            return new Response<string>(System.Net.HttpStatusCode.NotFound, "Apartment not found");
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
        _context.Apartments.Remove(find);
        await _context.SaveChangesAsync();
        return new Response<string>(System.Net.HttpStatusCode.OK, "Apartment deleted successfully");
    }
}
