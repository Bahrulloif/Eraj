

using Domain.DTOs.PictureDTO;

namespace Domain.DTOs.TransportDTOs.MotorbikeDTOs;

public class GetMotorbikeDTO : MotorbikeDTO
{
public List<PictureDto> Images { get; set; } = null!;
}
