using Domain.Enum;

namespace Domain.Entities;

public class Picture
{
    public int Id { get; set; }
    public ProductType ProductType { get; set; }
    public int ProductId { get; set; }
    public int SubCategoryId { get; set; }
    public string ImageName { get; set; } = null!;
}
