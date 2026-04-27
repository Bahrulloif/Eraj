using Domain.Enum.CarsEnum;
namespace Domain.Entities.Transport;


public class Car
{
    public int Id { get; set; }
    public int SubCategoryId { get; set; }
    public decimal Price { get; set; }
    public decimal DiscountPrice { get; set; }
    public int YearOfIssue { get; set; }
    public string Brand { get; set; } = null!;
    public string Model { get; set; } = null!;
    public string Engine { get; set; } = null!;
    public decimal EngineCapacity { get; set; }
    public int Mileage { get; set; } // в километрах
    public string Body { get; set; } = null!;
    public GearboxType Gearbox { get; set; }
    public string DriverUnit { get; set; } = null!; // можно заменить на enum, если есть фиксированный список
    public string ManufacturerState { get; set; } = null!;
    public FuelType FuelType { get; set; }
    public decimal FuelPer100km { get; set; }
    public int NumberOfSeats { get; set; }
    public Condition Condition { get; set; }
    public double? AccelerTo100km { get; set; } // в секундах
    public int? TrunkVolume { get; set; } // в литрах
    public int? Clearance { get; set; } // в мм
    public SteeringWheelSide? SteeringWheel { get; set; }
    public string? Color { get; set; }
    public string? InteriorColor { get; set; }
    public bool? PowerSteering { get; set; }
    public bool? SettingsMemory { get; set; }
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
