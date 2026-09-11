using Domain.DTOs.PictureDTO;
using Microsoft.AspNetCore.Http;

namespace Domain.DTOs.KomTechDTOs.TabletDTOs;

public class GetTabletDTO : TabletDTO
{
public List<PictureDto> Images { get; set; } = null!;
    // Who can edit/delete this listing (Businessman) - was never exposed to any client, making
    // "show me my own listings" impossible to build (see ../../../../../frontend/speca.md, Фаза 6).
    public string? OwnerId { get; set; }
}
