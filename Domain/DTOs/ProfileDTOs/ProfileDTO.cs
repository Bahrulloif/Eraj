using Domain.Entities;
using Domain.Enum;

namespace Domain.DTOs.ProfileDTO;

public class ProfileDTO
{
    public string Name { get; set; } = null!;
    public string Surname { get; set; } = null!;
    // Email/AddressId/CardNumber match ProfileUser's own nullability (made optional so
    // registration - which supplies none of these - doesn't crash). This DTO used to declare
    // them non-nullable, which broke UpdateProfile two different ways: Email non-nullable meant
    // a profile created via registration (Email = null in the DB) could never be updated again
    // without also inventing an email; AddressId as a plain `int` meant a client that simply
    // didn't set it defaulted to 0 (not null), which isn't a valid Address id and crashed
    // UpdateProfile with an unhandled DbUpdateException (FK_Profiles_Addresses_AddressId) -
    // reproduced live.
    public string? Email { get; set; }
    public string TelephoneNumber { get; set; } = null!;
    public DateTime Dob { get; set; }
    public string? Image { get; set; }
    public Gender Gender { get; set; }
    public int? AddressId { get; set; }
    public string? CardNumber { get; set; }
}
