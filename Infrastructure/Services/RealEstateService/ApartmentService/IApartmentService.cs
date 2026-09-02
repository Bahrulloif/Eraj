using Domain.DTOs.RealEstateDTOs.ApartmentDTOs;
using Domain.Filters.RealEstateFilters.ApartmentFilter;
using Domain.Responses;

namespace Infrastructure.Services.RealEstateService.ApartmentService;

public interface IApartmentService
{
    public Task<PagedResponse<List<GetApartmentDTO>>> GetApartment(GetApartmentFilter filter);
    public Task<Response<GetApartmentDTO>> GetApartmentById(int apartmentId);
    public Task<Response<string>> AddApartment(AddApartmentDTO apartment, string currentUserId);
    public Task<Response<string>> UpdateApartment(AddApartmentDTO apartment, string currentUserId, bool isPrivileged);
    public Task<Response<string>> DeleteApartment(int apartmentId, string currentUserId, bool isPrivileged);
}
