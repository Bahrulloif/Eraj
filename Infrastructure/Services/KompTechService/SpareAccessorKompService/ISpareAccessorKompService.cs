using Domain.DTOs.KomTechDTOs.SpareAccessorKompDTOs;
using Domain.DTOs.TransportDTOs.SpareAccessorKompDTOs;
using Domain.Filters.KompTechFilters.SpareAccessorKompFilter;
using Domain.Responses;

namespace Infrastructure.Services.KompTechService.SpareAccessorKompService;

public interface ISpareAccessorKompService
{

    public Task<PagedResponse<List<GetSpareAccessorKompDTO>>> GetSpareAccessorKomp(GetSpareAccessorKompFilter filter);
    public Task<Response<GetSpareAccessorKompDTO>> GetSpareAccessorKompById(int spareAccessorKompId);
    public Task<Response<string>> AddSpareAccessorKomp(AddSpareAccessorKompDTO spareAccessorKomp);
    public Task<Response<string>> UpdateSpareAccessorKomp(AddSpareAccessorKompDTO spareAccessorKomp);
    public Task<Response<string>> DeleteSpareAccessorKomp(int spareAccessorKompId);
}
