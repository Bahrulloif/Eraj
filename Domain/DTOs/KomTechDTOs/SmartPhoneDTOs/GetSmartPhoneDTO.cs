using Domain.DTOs.PictureDTO;
using Microsoft.AspNetCore.Http;

namespace Domain.DTOs.KomTechDTOs.SmartPhoneDTOs;

public class GetSmartPhoneDTO : SmartPhoneDTO
{
public List<PictureDto> Images { get; set; } = null!;
    // Who can edit/delete this listing (Businessman) - was never exposed to any client, making
    // "show me my own listings" impossible to build (see ../../../../../frontend/speca.md, Фаза 6).
    public string? OwnerId { get; set; }
}
