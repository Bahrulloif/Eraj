using Domain.Filters.MainFilter;

namespace Domain.Filters.KompTechFilters.SpareAccessorKompFilter;

public class GetSpareAccessorKompFilter : PaginationFilter
{
    public string? Name { get; set; }
    public int? SubCategoryId { get; set; }
    public string? OwnerId { get; set; }
}
