using Domain.Enum;

namespace Domain.Entities;

public class SubCategory
{
    public int SubCategoryId { get; set; }
    public string SubCategoryName { get; set; }=null!;
    public int CategoryId { get; set; }
    public Category Category { get; set; }=null!;
    // Which of the 11 product tables this subcategory's listings live in - without this, no
    // client could tell which /api/<ProductType> controller to query for "products in this
    // subcategory" (the same discriminator problem Picture/Order already had fixed for them, see
    // ../../../frontend/speca.md). Nullable: a subcategory doesn't have to be wired to a product
    // type the moment it's created (Admin picks one when it makes sense to).
    public ProductType? ProductType { get; set; }
}
