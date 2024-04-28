using Domain.DTOs.PictureDTO;

namespace Domain.DTOs.RatingAndTop;

public class RatingAndTopDTO
{
    public int Id { get; set; }
    public string Model { get; set; } = null!;
    public decimal Price { get; set; }
    public decimal DiscountPrice { get; set; }
    public List<PictureDto> Images { get; set; } = null!;

}
