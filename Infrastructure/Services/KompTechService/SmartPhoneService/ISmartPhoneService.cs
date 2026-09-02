using Domain.DTOs.KomTechDTOs.SmartPhoneDTOs;
using Domain.Filters.KompTechFilters.SmartPhoneFilters;
using Domain.Responses;

namespace Infrastructure.Services.KompTechService.SmartPhoneService;

public interface ISmartPhoneService
{
    public Task<PagedResponse<List<GetSmartPhoneDTO>>> GetSmartPhone(GetSmartPhoneFilter filter);
    public Task<Response<GetSmartPhoneDTO>> GetSmartPhoneById(int smartPhoneId);
    public Task<Response<string>> AddSmartPhone(AddSmartPhoneDTO smartPhone, string currentUserId);
    public Task<Response<string>> UpdateSmartPhone(AddSmartPhoneDTO smartPhone, string currentUserId, bool isPrivileged);
    public Task<Response<string>> DeleteSmartPhone(int smartPhoneId, string currentUserId, bool isPrivileged);
}
