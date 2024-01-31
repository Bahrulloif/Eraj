namespace Domain.DTOs.AddressDTO;

public class AddressDTO
{
    public int Id { get; set; }
    public string Country { get; set; } = null!;
    public string City { get; set; } = null!;
    public string Street { get; set; } = null!;
}
