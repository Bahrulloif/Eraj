using Domain.DTOs.PictureDTO;

namespace Domain.DTOs.RatingAndTopDTO;

public class RatingAndTopDTO
{
    public int ProductId { get; set; }
    public int SubCategoryId { get; set; }
    public string Model { get; set; } = null!;
    public decimal Price { get; set; }
    public decimal DiscountPrice { get; set; }
    public List<PictureDto> Images { get; set; } = null!;

}
