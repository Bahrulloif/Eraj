using Domain.DTOs.RoleDTO;
using Domain.DTOs.RoleDTOs;
using Domain.Filters.RoleFilter;
using Domain.Responses;

namespace Infrastructure.Services.RoleService;

public interface IRoleService
{
    Task<Response<List<RoleDto>>> GetRole(GetRoleFilter filter);
    Task<Response<RoleDto>> GetRoleById(string roleId);
    Task<Response<string>> AddRoleToUser(AddRoleToUserDto roleDTO);
    Task<Response<string>> DeleteRoleFromUser(AddRoleToUserDto roleDTO);
}
