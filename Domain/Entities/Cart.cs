using Domain.Enum;

namespace Domain.Entities;

public class Cart
{
    public int Id { get; set; }
    public ApplicationUser ApplicationUser { get; set; } = null!;
    public string ApplicationUserId { get; set; } = null!;
    public int SubCategoryId { get; set; }
    public int ProductId { get; set; }
    public DateTime DateOfPurchase { get; set; }
    public decimal? Amount { get; set; }
    public int Quantity { get; set; }
    // Which of the 11 product tables ProductId refers to - same discriminator Picture/Order
    // already have. SubCategoryId above existed on this entity the whole time but was never
    // mapped onto CartDTO in either direction, so it was always 0 in practice; both fixed
    // together here (see ../../../frontend/speca.md's Фаза 5 gap note for how this was found).
    public ProductType? ProductType { get; set; }

}
