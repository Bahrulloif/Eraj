namespace Domain.Entities;

public class Catalog
{
    public int CatalogId { get; set; }
    public string CatalogName { get; set; } = null!;
    public List<Category> Categories { get; set; } = null!;
}
