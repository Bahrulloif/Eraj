namespace Domain.Entities;

public class DeliveryAddress
{
    public int Id { get; set; }
    public ApplicationUser? ApplicationUser { get; set; }
    public int ApplicationUserId { get; set; }
    public Address? Address { get; set; }
    public int AddressId { get; set; }
    public List<Order>? Orders { get; set; }
}
