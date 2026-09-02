using Domain.DTOs.ProfileDTO;
using Domain.Filters.ProfileFilter;
using Domain.Responses;

namespace Infrastructure.Services.ProfileService;

public interface IProfileService
{
    Task<Response<List<GetProfileDTO>>> GetProfile(GetProfileFilter filter, string currentUserId, bool isPrivileged);
    Task<Response<GetProfileDTO>> GetProfileById(string profileId, string currentUserId, bool isPrivileged);
    Task<Response<GetProfileDTO>> AddProfile(AddProfileDTO profileDTO, string currentUserId);
    Task<Response<GetProfileDTO>> UpdateProfile(UpdateProfileDTO profile, string currentUserId, bool isPrivileged);
    Task<Response<GetProfileDTO>> DeleteProfile(string profileId, string currentUserId, bool isPrivileged);
}
