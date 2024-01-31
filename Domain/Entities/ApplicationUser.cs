using Microsoft.AspNetCore.Identity;

namespace Domain.Entities;

public class ApplicationUser : IdentityUser
{
    public List<Order>? Orders { get; set; }
    public ProfileUser ProfileUser { get; set; }=null!;
    public List<Cart>? Carts { get; set; }
    public List<DeliveryAddress>? DeliveryAddress { get; set; }
}
