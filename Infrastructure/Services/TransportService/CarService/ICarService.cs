using AutoMapper;
using Domain.DTOs.TransportDTOs.CarsDTOs;
using Domain.Filters.TransportFilters.CarsFilter;
using Domain.Responses;
using Infrastructure.Data;
using Infrastructure.Services.FileService;

namespace Infrastructure.Services.TransportService.CarService;

public interface ICarService
{
    public Task<PagedResponse<List<GetCarDTO>>> GetCar(GetCarFilter filter);
    public Task<Response<GetCarDTO>> GetCarById(int carId);
    public Task<Response<string>> AddCar(AddCarDTO car);
    public Task<Response<string>> UpdateCar(AddCarDTO car);
    public Task<Response<string>> DeleteCar(int carId);
}
