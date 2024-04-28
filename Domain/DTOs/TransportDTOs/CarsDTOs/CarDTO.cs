using Domain.Enum.CarsEnum;

namespace Domain.DTOs.TransportDTOs.CarsDTOs;

public class CarDTO
{
    public int Id { get; set; }
    public int SubCategoryId { get; set; }
    public decimal Price { get; set; }
    public decimal DiscountPrice { get; set; }
    public DateTime YearOfIssue { get; set; }
    public string Brand { get; set; } = null!;
    public string Engine { get; set; } = null!;
    public string Body { get; set; } = null!;
    public string Gearbox { get; set; } = null!;
    public string DriverUnit { get; set; } = null!;
    public string EngineCapacity { get; set; } = null!;
    public string Mileage { get; set; } = null!;
    public string ManufacturerState { get; set; } = null!;
    public string Model { get; set; } = null!;
    public string FuelPer100km { get; set; } = null!;
    public string NumberOfSeats { get; set; } = null!;
    public string Condition { get; set; } = null!;
    public string? AccelerTo100km { get; set; }
    public string? TrunkVolume { get; set; }
    public string? Clearance { get; set; }
    public string? SteeringWheel { get; set; }
    public string? Color { get; set; }
    public string? PowerSteering { get; set; }
    public string? InteriorColor { get; set; }
    public string? SettingsMemory { get; set; }
    public string? MultimediaAndNavigation { get; set; }
    public string? ClimateControl { get; set; }
    public string? DrivingAssistance { get; set; }
    public string? AntiTheftSystem { get; set; }
    public string? Airbags { get; set; }
    public string? Heating { get; set; }
    public string? TiresAndWheels { get; set; }
    public string? Headlights { get; set; }
    public string? AudioSystems { get; set; }
    public string? ElectricLifts { get; set; }
    public string? ElectricDrive { get; set; }
    public string? ActiveSafety { get; set; }
}
