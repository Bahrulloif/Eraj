using Domain.DTOs.RealEstateDTOs.CottageDTOs;
using Domain.Filters.RealEstateFilters.CottageFilter;
using Domain.Responses;

namespace Infrastructure.Services.RealEstateService.CottageService;

public interface ICottageService
{
    public Task<PagedResponse<List<GetCottageDTO>>> GetCottage(GetCottageFilter filter);
    public Task<Response<GetCottageDTO>> GetCottageById(int cottageId);
    public Task<Response<string>> AddCottage(AddCottageDTO cottage);
    public Task<Response<string>> UpdateCottage(AddCottageDTO cottage);
    public Task<Response<string>> DeleteCottage(int cottageId);
}
