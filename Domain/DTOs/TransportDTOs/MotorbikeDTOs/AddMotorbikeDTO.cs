using Microsoft.AspNetCore.Http;

namespace Domain.DTOs.TransportDTOs.MotorbikeDTOs;

public class AddMotorbikeDTO: MotorbikeDTO
{
        public List<IFormFile> Images { get; set; } = null!;
}
