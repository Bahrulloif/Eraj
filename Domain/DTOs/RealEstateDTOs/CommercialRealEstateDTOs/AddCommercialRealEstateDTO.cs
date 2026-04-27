using Microsoft.AspNetCore.Http;

namespace Domain.DTOs.RealEstateDTOs.CommercialRealEstateDTOs;

public class AddCommercialRealEstateDTO
{
    public List<IFormFile> Images { get; set; } = null!;
}
