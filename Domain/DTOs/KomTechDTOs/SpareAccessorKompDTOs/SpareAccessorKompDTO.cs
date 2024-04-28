namespace Domain.DTOs.TransportDTOs.SpareAccessorKompDTOs;

public class SpareAccessorKompDTO
{
    public int Id { get; set; }
    public string Model { get; set; } = null!;
    public string Description { get; set; } = null!;
    public decimal Price { get; set; }
    public decimal DiscountPrice { get; set; }
}
