namespace Domain.Entities.Transport;

public class SpareAccessorTransp
{
    public int Id { get; set; }
    public int SubCategoryId { get; set; }

    public string Model { get; set; } = null!;
    public string Description { get; set; } = null!;

    public decimal Price { get; set; }
    public decimal DiscountPrice { get; set; }

    // Дополнительно — при необходимости:
    public string? Brand { get; set; }
    public string? Compatibility { get; set; } // например, "Подходит для BMW X5 2015-2020"
    public string? Condition { get; set; } // Новая / Б/У — можно сделать enum
}
