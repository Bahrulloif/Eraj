namespace Domain.DTOs.CartDTO;

public class CartDTO
{
    public int Id { get; set; }
    public string UserId { get; set; } = null!;
    public int ProductId { get; set; }
    public DateTime DateOfPurchase { get; set; }
    public decimal? Amount { get; set; }
    public int Quantity { get; set; }
}
