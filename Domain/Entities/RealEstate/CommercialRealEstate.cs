using Domain.Enum.CommercialRealEstate;

namespace Domain.Entities.RealEstate;

public class CommercialRealEstate
{
    public int Id { get; set; }
    public int SubCategoryId { get; set; }
    public string? OwnerId { get; set; } // Businessman who created this listing; null for legacy rows managed only by Admin/SuperAdmin.
    public decimal Price { get; set; }
    public double Area { get; set; }
    public BuildingTypeEnum BuildingType { get; set; }
    public int Floor { get; set; }
}
