using Domain.DTOs.KomTechDTOs.SpareAccessorKompDTOs;
using Domain.DTOs.PictureDTO;

namespace Domain.DTOs.KomTechDTOs.SpareAccessorKompDTOs;

public class GetSpareAccessorKompDTO : SpareAccessorKompDTO
{
    public List<PictureDto> Images { get; set; } = null!;
}
