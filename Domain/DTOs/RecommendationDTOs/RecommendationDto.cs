using Microsoft.AspNetCore.Http;

namespace Domain.DTOs.RecommendationDTOs;

public class RecommendationDto
{
    public string Model { get; set; }
    public string Color { get; set; }
    public decimal Price { get; set; }
    public decimal DiscountPrice { get; set; }
    public string Picture { get; set; }
}
