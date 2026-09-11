using Domain.Enum;
using Domain.Filters.MainFilter;

namespace Domain.Filters.GetSubCategoryFilter;

public class GetSubCategoryFilter : PaginationFilter
{
    public string? Name { get; set; }
    public int? CategoryId { get; set; }
    public ProductType? ProductType { get; set; }
}
