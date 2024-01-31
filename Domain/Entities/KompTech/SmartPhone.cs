namespace Domain.Entities.KompTech;

public class SmartPhone
{
    public int Id { get; set; }
    public int SubCategoryId { get; set; }
    public string Model { get; set; } = null!;
    public string Core { get; set; } = null!;
    public int RAM { get; set; }
    public double Diagonal { get; set; }
    public int ROM { get; set; }
    public decimal Price { get; set; }
    public decimal DiscountPrice { get; set; }
    public string Color { get; set; }=null!;
    public string Picture { get; set; }=null!;
}
