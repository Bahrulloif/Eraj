using Domain.DTOs.PictureDTO;

namespace Domain.DTOs.TransportDTOs.TruckDTOs;

public class GetTruckDTO : TruckDTO
{
    public List<PictureDto> Images { get; set; } = null!;
}
