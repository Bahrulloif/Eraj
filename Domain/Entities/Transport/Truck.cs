namespace Domain.Entities.Transport;

public class Truck
{
    public int Id { get; set; }
    public int SubCategoryId { get; set; }
    public decimal Price { get; set; }
    public decimal PriceDiscount { get; set; }
    public string Brand { get; set; } = null!;
    public string Model { get; set; } = null!;
    public DateTime YearOfIssue { get; set; }
    public string? BodyType { get; set; }
    public string Power { get; set; } = null!;
    public string EngineType { get; set; } = null!;
    public string EngineCapacity { get; set; } = null!;
    public string? EnvironmentalClass { get; set; }
    public string Transmission { get; set; } = null!;
    public string? WheelFormula { get; set; }
    public string LoadCapacity { get; set; } = null!;
    public string PermittedMaximumWeight { get; set; } = null!;
    public string Mileage { get; set; } = null!;
}
