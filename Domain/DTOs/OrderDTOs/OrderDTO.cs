using System.ComponentModel.DataAnnotations;
using Domain.Enum;

namespace Domain.DTOs.OrderDTO;

public class OrderDTO
{
    public int Id { get; set; }
    public string UserId { get; set; } = null!;
    // The caller already knows this - they fetched the listing from a type-specific endpoint
    // (e.g. GET /api/Car/get/carById) before ordering it. AddOrder/UpdateOrder use it to look up
    // the product's real price server-side (see OrderService.GetRealPrice) instead of trusting
    // whatever Price the client sends - see Price below.
    public ProductType ProductType { get; set; }
    public int ProductId { get; set; }
    public int SubCategoryId { get; set; }
    public string Model { get; set; } = null!;
    [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
    public int Quantity { get; set; }
    // Ignored on write: AddOrder/UpdateOrder always overwrite this with the real product price
    // looked up server-side by (ProductType, ProductId, SubCategoryId) - a client-submitted Price
    // here was previously trusted as-is, letting anyone order anything at any price they named
    // (confirmed live: ordered a 24000 car for 0.01). Still present on the DTO because GetOrderDTO
    // reuses this class to report back what was actually charged.
    public decimal Price { get; set; }
    public DateTime OrderDate { get; set; }
    public OrderStatus OrderStatus { get; set; }
    public int DeliveryAddressId { get; set; }
}
