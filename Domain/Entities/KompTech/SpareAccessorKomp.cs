using Domain.Enum.SpareAccessorKompEnum;

namespace Domain.Entities.KompTech;

public class SpareAccessorKomp
{
    public int Id { get; set; }
    public int SubCategoryId { get; set; }

    public string Model { get; set; } = null!;
    public string Description { get; set; } = null!;

    public decimal Price { get; set; }
    public decimal DiscountPrice { get; set; }

    // Дополнительно:
    public string? Brand { get; set; }
    public string? Compatibility { get; set; }
    public ConditionType? Condition { get; set; }
}
