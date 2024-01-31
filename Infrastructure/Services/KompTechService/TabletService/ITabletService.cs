using Domain.DTOs.KomTechDTOs.TabletDTOs;
using Domain.Filters.KompTechFilters.TabletFilter;
using Domain.Responses;

namespace Infrastructure.Services.KompTechService.TabletService;

public interface ITabletService
{
    public Task<Response<List<GetTabletDTO>>> GetTablet(GetTabletFilter filter);
    public Task<Response<GetTabletDTO>> GetTabletById(int tabletId);
    public Task<Response<GetTabletDTO>> AddTablet(AddTabletDTO tablet);
    public Task<Response<GetTabletDTO>> UpdateTablet(AddTabletDTO tablet);
    public Task<Response<GetTabletDTO>> DeleteTablet(int tabletId);
}
