using Domain.DTOs.PictureDTO;
using Microsoft.AspNetCore.Http;

namespace Domain.DTOs.RecommendationDTOs;

public class RecommendationDto
{
    public string Model { get; set; } = null!;
    public string Color { get; set; } = null!;
    public decimal Price { get; set; }
    public decimal DiscountPrice { get; set; }
    public List<PictureDto> Image { get; set; } = null!;
}
