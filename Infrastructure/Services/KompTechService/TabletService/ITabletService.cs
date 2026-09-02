using Domain.DTOs.KomTechDTOs.TabletDTOs;
using Domain.Filters.KompTechFilters.TabletFilter;
using Domain.Responses;

namespace Infrastructure.Services.KompTechService.TabletService;

public interface ITabletService
{
    public Task<PagedResponse<List<GetTabletDTO>>> GetTablet(GetTabletFilter filter);
    public Task<Response<GetTabletDTO>> GetTabletById(int tabletId);
    public Task<Response<string>> AddTablet(AddTabletDTO tablet, string currentUserId);
    public Task<Response<string>> UpdateTablet(AddTabletDTO tablet, string currentUserId, bool isPrivileged);
    public Task<Response<string>> DeleteTablet(int tabletId, string currentUserId, bool isPrivileged);
}
