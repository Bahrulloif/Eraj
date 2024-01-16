using Domain.DTOs.KomTechDTOs.SmartPhoneDTOs;
using Domain.Filters.KompTechFilters.SmartPhoneFilters;
using Domain.Responses;

namespace Infrastructure.Services.KompTechService.SmartPhoneService;

public interface ISmartPhoneService
{
    public Task<Response<List<GetSmartPhoneDTO>>> GetSmartPhone(GetSmartPhoneFilter filter);
    public Task<Response<GetSmartPhoneDTO>> GetSmartPhoneById(int smartPhoneId);
    public Task<Response<GetSmartPhoneDTO>> AddSmartPhone(AddSmartPhoneDTO smartPhone);
    public Task<Response<GetSmartPhoneDTO>> UpdateSmartPhone(AddSmartPhoneDTO smartPhone);
    public Task<Response<GetSmartPhoneDTO>> DeleteSmartPhone(int smartPhoneId);
}
