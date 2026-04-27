namespace Domain.DTOs.RealEstateDTOs.CottageDTOs;

public class AddCottageDTO : CottageDTO
{
     public List<IFormFile> Images { get; set; } = null!;
}
