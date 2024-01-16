using System.ComponentModel.DataAnnotations;
using Domain.Enum;

namespace Domain.Entities;

public class ProfileUser
{
    [Key]
    public string ApplicationUserId { get; set; } = null!;
    public ApplicationUser ApplicationUser { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Surname { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string TelephoneNumber { get; set; } = null!;
    public DateTime Dob { get; set; }
    public string? Image { get; set; }
    public Gender Gender { get; set; }
    public Address Address { get; set; } = null!;
    public int AddressId { get; set; }
    public List<DeliveryAddress>? DeliveryAddress { get; set; }
    public string CardNumber { get; set; } = null!;

}
