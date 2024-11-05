using Microsoft.AspNetCore.Http;

namespace Domain.DTOs.KomTechDTOs.SpareAccessorKompDTOs;

public class AddSpareAccessorKompDTO : SpareAccessorKompDTO
{
    public List<IFormFile> Images { get; set; } = null!;
}
