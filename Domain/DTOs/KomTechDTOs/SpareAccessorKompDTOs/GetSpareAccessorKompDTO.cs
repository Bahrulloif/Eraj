using Domain.DTOs.KomTechDTOs.SpareAccessorKompDTOs;
using Domain.DTOs.PictureDTO;

namespace Domain.DTOs.TransportDTOs.SpareAccessorKompDTOs;

public class GetSpareAccessorKompDTO : SpareAccessorKompDTO
{
    public List<PictureDto> Images { get; set; } = null!;
}
