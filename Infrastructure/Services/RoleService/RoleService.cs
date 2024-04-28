using Domain.DTOs.RoleDTO;
using Domain.DTOs.RoleDTOs;
using Domain.Entities;
using Domain.Filters.RoleFilter;
using Domain.Responses;
using Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.RoleService;

public class RoleService : IRoleService
{
    private readonly RoleManager<Roles> _roleManager;
    private readonly DataContext _dataContext;
    private readonly UserManager<ApplicationUser> _userManager;

    public RoleService(RoleManager<Roles> roleManager, DataContext dataContext, UserManager<ApplicationUser> userManager)
    {
        _roleManager = roleManager;
        _dataContext = dataContext;
        _userManager = userManager;
    }

    public async Task<Response<List<RoleDto>>> GetRole(GetRoleFilter filter)
    {
        var roles = await _dataContext.Roles.Select(x => new RoleDto
        {
            RoleId = x.Id,
            RoleName = x.Name!
        }).ToListAsync();
        return new Response<List<RoleDto>>(roles);
    }

    public async Task<Response<RoleDto>> GetRoleById(string roleId)
    {
        var role = await _dataContext.Roles.Select(x => new RoleDto
        {
            RoleId = x.Id,
            RoleName = x.Name!
        }).FirstOrDefaultAsync(x => x.RoleId == roleId);
        if (role == null)
            return new Response<RoleDto>(System.Net.HttpStatusCode.NotFound, "Role not found");
        return new Response<RoleDto>(role);
    }

    public async Task<Response<string>> AddRoleToUser(AddRoleToUserDto roleDTO)
    {
        var user = await _userManager.FindByIdAsync(roleDTO.UserId);
        if (user == null)
            return new Response<string>(System.Net.HttpStatusCode.NotFound, "User not found");
        var role = await _roleManager.FindByIdAsync(roleDTO.RoleId);
        if (role == null)
            return new Response<string>(System.Net.HttpStatusCode.NotFound, "Role not Found");
        await _userManager.AddToRoleAsync(user, role.Name);

        return new Response<string>("Role added to user successfully");
    }

    public async Task<Response<string>> DeleteRoleFromUser(AddRoleToUserDto roleDTO)
    {
        var user = await _userManager.FindByIdAsync(roleDTO.UserId);
        if (user == null)
            return new Response<string>(System.Net.HttpStatusCode.NotFound, "User not found");
        var role = await _roleManager.FindByIdAsync(roleDTO.RoleId);
        if (role == null)
            return new Response<string>(System.Net.HttpStatusCode.NotFound, "Role not Found");
        await _userManager.RemoveFromRoleAsync(user, role.Name);
        return new Response<string>("Role removed from user successfully");
    }
}
