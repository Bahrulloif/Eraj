using Domain.Enum.CommercialRealEstate;

namespace Domain.Entities.RealEstate;

public class CommercialRealEstate
{
    public int Id { get; set; }
    public decimal Price { get; set; }
    public double Area { get; set; }
    public BuildingTypeEnum BuildingType { get; set; }
    public int Floor { get; set; }
}
