namespace Infrastructure.Services.RoleService;

public interface IRoleService
{
    Task<Response<List<GetRoleDTO>>> GetRole(GetRoleFilter filter);
    Task<Response<GetRoleDTO>> GetRoleById(string roleId);
    Task<Response<GetRoleDTO>> AddRole(AddRoleDTO roleDTO);
    Task<Response<GetRoleDTO>> UpdateRole(UpdateRoleDTO role);
    Task<Response<GetRoleDTO>> DeleteRole(string roleId);
}
