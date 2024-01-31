namespace Domain.Entities;

public class Category
{
    public int CategoryId { get; set; }
    public string CategoryName { get; set; } = null!;
    public int CatalogId { get; set; }
    public Catalog Catalog { get; set; }= null!;
    public List<SubCategory> SubCategories { get; set; } = null!;

}
