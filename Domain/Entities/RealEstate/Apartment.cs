using Domain.Enum.Apartment;

namespace Domain.Entities.RealEstate;

public class Apartment
{
    public int Id { get; set; }
    public int NumberOfRooms { get; set; }
    public decimal Price { get; set; }
    public decimal PricePerM2 { get; set; }
    public float TotalArea { get; set; }
    public int Floor { get; set; }
    public RenovationTypeEnum Renovation { get; set; }
    public float CeilingHeight { get; set; }
    public int YearOfHouseBuild { get; set; }
    public int FloorsInTheHouse { get; set; }
    public bool Parking { get; set; }
    public float KitchenArea { get; set; }
    public bool IsNewBuilding { get; set; }
}
