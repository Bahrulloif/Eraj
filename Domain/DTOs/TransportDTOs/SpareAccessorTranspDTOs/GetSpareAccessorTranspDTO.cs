using Domain.DTOs.PictureDTO;

namespace Domain.DTOs.TransportDTOs.SpareAccessorTranspDTOs;

public class GetSpareAccessorTranspDTO : SpareAccessorTranspDTO
{
    public List<PictureDto> Images { get; set; } = null!;
}
