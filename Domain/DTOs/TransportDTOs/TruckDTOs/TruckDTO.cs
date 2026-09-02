using Domain.Enum.TruckEnum;

namespace Domain.DTOs.TransportDTOs.TruckDTOs;

public class TruckDTO
{
    public int Id { get; set; }
    public int SubCategoryId { get; set; }
    public decimal Price { get; set; }
    public decimal PriceDiscount { get; set; }
    public string Brand { get; set; } = null!;
    public string Model { get; set; } = null!;
    public int YearOfIssue { get; set; }
    public BodyType? BodyType { get; set; }
    public int Power { get; set; }
    public EngineType EngineType { get; set; }
    public decimal EngineCapacity { get; set; }
    public string? EnvironmentalClass { get; set; }
    public TransmissionType Transmission { get; set; }
    public string? WheelFormula { get; set; }
    public decimal LoadCapacity { get; set; }
    public decimal PermittedMaximumWeight { get; set; }
    public int Mileage { get; set; }
}
