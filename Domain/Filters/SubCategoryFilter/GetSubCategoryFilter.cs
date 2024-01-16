using Domain.Filters.MainFilter;

namespace Domain.Filters.GetSubCategoryFilter;

public class GetSubCategoryFilter : PaginationFilter
{
    public string? Name { get; set; }
}
