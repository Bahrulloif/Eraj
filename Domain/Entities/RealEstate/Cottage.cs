using Domain.Enum.Apartment;
using Domain.Enum.CottageEnum;

namespace Domain.Entities.RealEstate;

public class Cottage
{
    public int Id { get; set; }
    public int SubCategoryId { get; set; }
    public string? OwnerId { get; set; } // Businessman who created this listing; null for legacy rows managed only by Admin/SuperAdmin.
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