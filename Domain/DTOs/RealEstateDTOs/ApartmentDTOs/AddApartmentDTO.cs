namespace Domain.DTOs.RealEstateDTOs.ApartmentDTOs;

public class AddApartmentDTO : ApartmentDTO
{
    public List<IFormFile> Images { get; set; } = null!;
}
