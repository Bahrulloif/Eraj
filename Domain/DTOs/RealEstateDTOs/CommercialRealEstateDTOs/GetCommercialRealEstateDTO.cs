using Domain.DTOs.PictureDTO;

namespace Domain.DTOs.RealEstateDTOs.CommercialRealEstateDTOs;

public class GetCommercialRealEstateDTO : CommercialRealEstateDTO
{
    public List<PictureDto> Images { get; set; } = null!;
}
