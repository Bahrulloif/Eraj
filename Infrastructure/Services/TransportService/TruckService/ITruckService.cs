using Domain.DTOs.TransportDTOs.TruckDTOs;
using Domain.Filters.TransportFilter.TruckFilters;
using Domain.Responses;

namespace Infrastructure.Services.TransportService.TruckService;

public interface ITruckService
{
    public Task<PagedResponse<List<GetTruckDTO>>> GetTruck(TruckFilter filter);
    public Task<Response<GetTruckDTO>> GetTruckById(int truckId);
    public Task<Response<string>> AddTruck(AddTruckDTO truck);
    public Task<Response<string>> UpdateTruck(AddTruckDTO truck);
    public Task<Response<string>> DeleteTruck(int truckId);
}
