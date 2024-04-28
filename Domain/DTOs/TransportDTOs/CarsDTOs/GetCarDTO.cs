using Domain.DTOs.PictureDTO;

namespace Domain.DTOs.TransportDTOs.CarsDTOs;

public class GetCarDTO: CarDTO
{
    public List<PictureDto> Images { get; set; } = null!;
}
