namespace Domain.Entities;

public class Address
{
    public int Id { get; set; }
    public string? OwnerId { get; set; } // who created it; covers the window before it's linked to a Profile/DeliveryAddress.
    public string Country { get; set; } = null!;
    public string City { get; set; } = null!;
    public string Street { get; set; } = null!;
    public List<ProfileUser>? ProfileUsers { get; set; }
    public List<DeliveryAddress>? DeliveryAddresses { get; set; }
    

}
