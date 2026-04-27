using Domain.Enum.TruckEnum;

namespace Domain.Entities.Transport;

public class Truck
{
    public int Id { get; set; }
    public int SubCategoryId { get; set; }

    public decimal Price { get; set; }
    public decimal PriceDiscount { get; set; }

    public string Brand { get; set; } = null!;
    public string Model { get; set; } = null!;
    public int YearOfIssue { get; set; }

    public BodyType? BodyType { get; set; }
    public int Power { get; set; } // в л.с.
    public EngineType EngineType { get; set; }
    public decimal EngineCapacity { get; set; } // литры или куб. см — зависит от требований

    public string? EnvironmentalClass { get; set; } // может быть Enum, если есть фиксированные значения

    public TransmissionType Transmission { get; set; }
    public string? WheelFormula { get; set; } // например: 4x2, 6x4 и т.п.

    public decimal LoadCapacity { get; set; } // в тоннах или кг
    public decimal PermittedMaximumWeight { get; set; } // в тоннах или кг

    public int Mileage { get; set; } // в километрах
}
