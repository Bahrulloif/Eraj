using Domain.Enum.Apartment;
using Domain.Enum.CottageEnum;

namespace Domain.Entities.RealEstate;

public class Cottage
{
    public int Id { get; set; }
    public TypeOfEstateEnum TypeOfRealEstate { get; set; }
    public decimal Price { get; set; }
    public double PricePerM2 { get; set; }
    public double HouseArea { get; set; }
    public double PlotArea { get; set; }
    public RenovationTypeEnum Renovation { get; set; }
    public int NumberOfRooms { get; set; }
    public WallMaterialEnum WallMaterial { get; set; }
    public bool Parking { get; set; }
}