using Domain.DTOs.PictureDTO;
using Microsoft.AspNetCore.Http;

namespace Domain.DTOs.KomTechDTOs.SmartPhoneDTOs;

public class GetSmartPhoneDTO : SmartPhoneDTO
{
public List<PictureDto> Images { get; set; } = null!;
}
