using Domain.Entities;
using Domain.Enum;

namespace Domain.DTOs.ProfileDTO;

public class ProfileDTO
{
    public string Name { get; set; } = null!;
    public string Surname { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string TelephoneNumber { get; set; } = null!;
    public DateTime Dob { get; set; }
    public string? Image { get; set; }
    public Gender Gender { get; set; }
    public Address Address { get; set; } = null!;
    public string CardNumber { get; set; } = null!;
}
