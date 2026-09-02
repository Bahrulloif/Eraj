using Domain.Enum.SpareAccessorKompEnum;

namespace Domain.Entities.KompTech;

public class SpareAccessorKomp
{
    public int Id { get; set; }
    public int SubCategoryId { get; set; }
    public string? OwnerId { get; set; } // Businessman who created this listing; null for legacy rows managed only by Admin/SuperAdmin.

    public string Model { get; set; } = null!;
    public string Description { get; set; } = null!;

    public decimal Price { get; set; }
    public decimal DiscountPrice { get; set; }

    // Дополнительно:
    public string? Brand { get; set; }
    public string? Compatibility { get; set; }
    public ConditionType? Condition { get; set; }
}
