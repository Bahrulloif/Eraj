using Domain.Enum;

namespace Domain.DTOs.OrderDTO;

public class OrderDTO
{
    public int Id { get; set; }
    public string UserId { get; set; } = null!;
    public int ProductId { get; set; }
    public int SubCategoryId { get; set; }
    public string Model { get; set; } = null!;
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public DateTime OrderDate { get; set; }
    public OrderStatus OrderStatus { get; set; }
    public int DeliveryAddressId { get; set; }
}
