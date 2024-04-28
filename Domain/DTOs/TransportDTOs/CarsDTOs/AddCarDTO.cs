using Microsoft.AspNetCore.Http;

namespace Domain.DTOs.TransportDTOs.CarsDTOs;

public class AddCarDTO : CarDTO
{
    public List<IFormFile> Images { get; set; } = null!;
}
