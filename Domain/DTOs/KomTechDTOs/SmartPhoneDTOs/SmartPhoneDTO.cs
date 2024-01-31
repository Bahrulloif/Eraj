using System.ComponentModel.DataAnnotations;

namespace Domain.DTOs.KomTechDTOs.SmartPhoneDTOs;

public class SmartPhoneDTO
{
    public int Id { get; set; }
    [Required]
    public string Model { get; set; } = null!;
    public string Core { get; set; } = null!;
    [Required]
    public int RAM { get; set; }
    public double Diagonal { get; set; }
    [Required]
    public int ROM { get; set; }
    public decimal Price { get; set; }
    public decimal DiscountPrice { get; set; }
    public string Color { get; set; } = null!;
    public string Picture { get; set; } = null!;
}
