using Microsoft.AspNetCore.Http;

namespace Domain.DTOs.KomTechDTOs.SmartPhoneDTOs;

public class AddSmartPhoneDTO : SmartPhoneDTO
{
     public List<IFormFile> Images { get; set; } = null!;
}
