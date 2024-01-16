using Domain.DTOs.ProfileDTO;
using Domain.Filters.ProfileFilter;
using Domain.Responses;

namespace Infrastructure.Services.ProfileService;

public interface IProfileService
{
    Task<Response<List<GetProfileDTO>>> GetProfile(GetProfileFilter filter);
    Task<Response<GetProfileDTO>> GetProfileById(string profileId);
    Task<Response<GetProfileDTO>> UpdateProfile(UpdateProfileDTO profile);
    Task<Response<GetProfileDTO>> DeleteProfile(string profileId);
}
