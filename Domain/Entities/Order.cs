using Domain.Enum;

namespace Domain.Entities;

public class Order
{
    public int Id { get; set; }
    public string UserId { get; set; } = null!;
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public DateTime OrderDate { get; set; }
    public OrderStatus OrderStatus { get; set; }
    public int DeliveryAddressId { get; set; }
    public DeliveryAddress? DeliveryAddress { get; set; }
}