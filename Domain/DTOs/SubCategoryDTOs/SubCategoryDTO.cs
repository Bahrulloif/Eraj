using Domain.Enum;

namespace Domain.DTOs.SubCategoryDTOs;

public class SubCategoryDTO
{
     public int SubCategoryId { get; set; }
    public string SubCategoryName { get; set; }=null!;
    public int CategoryId { get; set; }
    public ProductType? ProductType { get; set; }
}
