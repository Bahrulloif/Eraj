namespace Domain.Entities;

public class SubCategory
{
    public int SubCategoryId { get; set; }
    public string SubCategoryName { get; set; }=null!;
    public int CategoryId { get; set; }
    public Category Category { get; set; }=null!;
}
