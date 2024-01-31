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

}
