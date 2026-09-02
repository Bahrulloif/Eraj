using Microsoft.AspNetCore.Http;

namespace Domain.DTOs.RealEstateDTOs.CommercialRealEstateDTOs;

public class AddCommercialRealEstateDTO : CommercialRealEstateDTO
{
    public List<IFormFile> Images { get; set; } = null!;
}
