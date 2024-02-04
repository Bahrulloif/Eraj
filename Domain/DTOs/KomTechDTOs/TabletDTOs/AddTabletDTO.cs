using Microsoft.AspNetCore.Http;

namespace Domain.DTOs.KomTechDTOs.TabletDTOs;

public class AddTabletDTO : TabletDTO
{
    public List<IFormFile> Images { get; set; } = null!;
}
