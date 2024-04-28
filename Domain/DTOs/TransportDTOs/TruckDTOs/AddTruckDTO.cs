using Microsoft.AspNetCore.Http;

namespace Domain.DTOs.TransportDTOs.TruckDTOs;

public class AddTruckDTO : TruckDTO
{
     public List<IFormFile> Images { get; set; } = null!;
}
