using Domain.DTOs.PictureDTO;

namespace Domain.DTOs.RealEstateDTOs.CottageDTOs;

public class GetCottageDTO : CottageDTO
{
    public List<PictureDto> Images { get; set; } = null!;
}
