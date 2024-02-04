using Domain.DTOs.PictureDTO;
using Microsoft.AspNetCore.Http;

namespace Domain.DTOs.KomTechDTOs.TabletDTOs;

public class GetTabletDTO : TabletDTO
{
public List<PictureDto> Images { get; set; } = null!;
}
