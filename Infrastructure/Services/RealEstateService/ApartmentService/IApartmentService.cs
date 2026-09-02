using Domain.DTOs.RealEstateDTOs.ApartmentDTOs;
using Domain.Filters.RealEstateFilters.ApartmentFilter;
using Domain.Responses;

namespace Infrastructure.Services.RealEstateService.ApartmentService;

public interface IApartmentService
{
    public Task<PagedResponse<List<GetApartmentDTO>>> GetApartment(GetApartmentFilter filter);
    public Task<Response<GetApartmentDTO>> GetApartmentById(int apartmentId);
    public Task<Response<string>> AddApartment(AddApartmentDTO apartment);
    public Task<Response<string>> UpdateApartment(AddApartmentDTO apartment);
    public Task<Response<string>> DeleteApartment(int apartmentId);
}
