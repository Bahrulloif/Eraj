using Domain.DTOs.PictureDTO;

namespace Domain.DTOs.RealEstateDTOs.CottageDTOs;

public class GetCottageDTO
{
    public List<PictureDto> Images { get; set; } = null!;
}
