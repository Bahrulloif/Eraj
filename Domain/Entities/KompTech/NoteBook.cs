using Domain.Enum.NoteBookEnum;

namespace Domain.Entities.KompTech;

public class NoteBook
{
    public int Id { get; set; }
    public int SubCategoryId { get; set; }

    public string Model { get; set; } = null!;
    public CpuBrand Core { get; set; }

    public int RAM { get; set; } // в ГБ
    public int ROM { get; set; } // в ГБ

    public double Diagonal { get; set; } // в дюймах
    public string Color { get; set; } = null!;

    public decimal Price { get; set; }
    public decimal DiscountPrice { get; set; }

    // Дополнительные поля (по желанию)
    public string? ProcessorName { get; set; }
    public string? GPU { get; set; }
    public string? ScreenResolution { get; set; }
    public string? OS { get; set; }
}
