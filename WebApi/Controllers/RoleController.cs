using Domain.DTOs.RoleDTO;
using Domain.DTOs.RoleDTOs;
using Domain.Entities;
using Domain.Filters.RoleFilter;
using Domain.Responses;
using Infrastructure.Services.RoleService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RoleController : BaseController
{
    private readonly IRoleService _roleService;

    public RoleController(IRoleService roleService)
    {
        _roleService = roleService;
    }

    [HttpGet("GetRole")]
    [Authorize(Roles = "SuperAdmin")]
    public async Task<ActionResult> GetRole([FromQuery]GetRoleFilter filter)
    {
        var result = await _roleService.GetRole(filter);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("GetRoleById")]
    [Authorize(Roles = "SuperAdmin")]
    public async Task<IActionResult> GetRoleById(string roleId)
    {
        var result = await _roleService.GetRoleById(roleId);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPost("AddRoleToUser"), Authorize(Roles = "SuperAdmin")]
    public async Task<IActionResult> AddRoleToUser(AddRoleToUserDto roleDTO)
    {
        var result = await _roleService.AddRoleToUser(roleDTO);
        return StatusCode(result.StatusCode, result);
    }

    [HttpDelete("DeleteRoleFromUser"), Authorize(Roles = "SuperAdmin")]
    public async Task<IActionResult> DeleteRoleFromUser(AddRoleToUserDto roleDTO)
    {
        var result = await _roleService.DeleteRoleFromUser(roleDTO);
        return StatusCode(result.StatusCode, result);
    }
}
