using Domain.DTOs.TransportDTOs.SpareAccessorTranspDTOs;
using Domain.Filters.TransportFilter.SpareAccessorTranspFilters;
using Domain.Responses;

namespace Infrastructure.Services.TransportService.SpareAccessorTranspService;

public interface ISpareAccessorTranspService
{
    public Task<PagedResponse<List<GetSpareAccessorTranspDTO>>> GetSpareAccessorTransp(GetSpareAccessorTranspFilter filter);
    public Task<Response<GetSpareAccessorTranspDTO>> GetSpareAccessorTranspById(int SpareAccessorTranspId);
    public Task<Response<string>> AddSpareAccessorTransp(AddSpareAccessorTranspDTO SpareAccessorTransp);
    public Task<Response<string>> UpdateSpareAccessorTransp(AddSpareAccessorTranspDTO SpareAccessorTransp);
    public Task<Response<string>> DeleteSpareAccessorTransp(int SpareAccessorTranspId);
}
