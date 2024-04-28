using Microsoft.AspNetCore.Http;

namespace Domain.DTOs.TransportDTOs.SpareAccessorTranspDTOs;

public class AddSpareAccessorTranspDTO : SpareAccessorTranspDTO
{
    public List<IFormFile> Images { get; set; } = null!;
}
