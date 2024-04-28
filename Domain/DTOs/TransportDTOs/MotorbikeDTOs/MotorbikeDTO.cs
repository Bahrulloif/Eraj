namespace Domain.DTOs.TransportDTOs.MotorbikeDTOs;

public class MotorbikeDTO
{
    public int Id { get; set; }
    public int SubCategoryId { get; set; }
    public string Model { get; set; } = null!;
    public string Region { get; set; } = null!;
    public decimal Price { get; set; }
    public decimal DiscountPrice { get; set; }
    public string Brand { get; set; } = null!;
    public DateTime YearOfIssue { get; set; }
    public string? ManufacturerCountry { get; set; }
    public string? EngineType { get; set; }
    public string? EngineCapacity { get; set; }
    public string? Power { get; set; }
    public string? FuelSupply { get; set; }
    public string? NumberOfCycles { get; set; }
    public string? GearBox { get; set; }
    public string? Mileage { get; set; }
    public string? Passengers { get; set; }
}
