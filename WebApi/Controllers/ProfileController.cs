using Domain.DTOs.ProfileDTO;
using Domain.Filters.ProfileFilter;
using Domain.Responses;
using Infrastructure.Services.ProfileService;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProfileController : BaseController
{
    private readonly IProfileService _profileService;
    public ProfileController(IProfileService profileService)
    {
        _profileService = profileService;
    }

    [HttpGet("get/profile")]
    public async Task<IActionResult> GetProfile([FromQuery]GetProfileFilter filter)
    {
        if (ModelState.IsValid)
        {
            var result = await _profileService.GetProfile(filter, CurrentUserId!, IsPrivilegedUser);
            return StatusCode(result.StatusCode, result);
        }
        var response = new Response<List<GetProfileDTO>>(System.Net.HttpStatusCode.BadRequest, ModelStateErrors());
        return StatusCode(response.StatusCode, response);
    }
    [HttpGet("get/profileById")]
    public async Task<IActionResult> GetProfileById(string profileId)
    {
        if (ModelState.IsValid)
        {
            var result = await _profileService.GetProfileById(profileId, CurrentUserId!, IsPrivilegedUser);
            return StatusCode(result.StatusCode, result);
        }
        var response = new Response<GetProfileDTO>(System.Net.HttpStatusCode.BadRequest, ModelStateErrors());
        return StatusCode(response.StatusCode, response);
    }
    [HttpPost("post/profile")]
    public async Task<IActionResult> AddProfile(AddProfileDTO profile)
    {
        if (ModelState.IsValid)
        {
            var result = await _profileService.AddProfile(profile, CurrentUserId!);
            return StatusCode(result.StatusCode, result);
        }
        var response = new Response<GetProfileDTO>(System.Net.HttpStatusCode.BadRequest, ModelStateErrors());
        return StatusCode(response.StatusCode, response);
    }
    [HttpPut("put/profile")]
    public async Task<IActionResult> UpdateProfile(UpdateProfileDTO profile)
    {
        if (ModelState.IsValid)
        {
            var result = await _profileService.UpdateProfile(profile, CurrentUserId!, IsPrivilegedUser);
            return StatusCode(result.StatusCode, result);
        }
        var response = new Response<GetProfileDTO>(System.Net.HttpStatusCode.BadRequest, ModelStateErrors());
        return StatusCode(response.StatusCode, response);
    }
    [HttpDelete("delete/profile")]
    public async Task<IActionResult> DeleteProfile(string profileId)
    {
        if (ModelState.IsValid)
        {
            var result = await _profileService.DeleteProfile(profileId, CurrentUserId!, IsPrivilegedUser);
            return StatusCode(result.StatusCode, result);
        }
        var response = new Response<GetProfileDTO>(System.Net.HttpStatusCode.BadRequest, ModelStateErrors());
        return StatusCode(response.StatusCode, response);
    }
}
