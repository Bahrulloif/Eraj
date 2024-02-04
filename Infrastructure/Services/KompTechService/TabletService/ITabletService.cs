using Domain.DTOs.KomTechDTOs.TabletDTOs;
using Domain.Filters.KompTechFilters.TabletFilter;
using Domain.Responses;

namespace Infrastructure.Services.KompTechService.TabletService;

public interface ITabletService
{
    public Task<PagedResponse<List<GetTabletDTO>>> GetTablet(GetTabletFilter filter);
    public Task<Response<GetTabletDTO>> GetTabletById(int tabletId);
    public Task<Response<string>> AddTablet(AddTabletDTO tablet);
    public Task<Response<string>> UpdateTablet(AddTabletDTO tablet);
    public Task<Response<string>> DeleteTablet(int tabletId);
}
