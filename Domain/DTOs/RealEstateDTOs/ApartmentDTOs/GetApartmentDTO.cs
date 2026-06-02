using Domain.DTOs.PictureDTO;

namespace Domain.DTOs.RealEstateDTOs.ApartmentDTOs;

public class GetApartmentDTO : ApartmentDTO
{
    public List<PictureDto> Images { get; set; } = null!;
}
