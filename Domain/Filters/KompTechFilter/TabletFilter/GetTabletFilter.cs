using Domain.Filters.MainFilter;

namespace Domain.Filters.KompTechFilters.TabletFilter;

public class GetTabletFilter : PaginationFilter
{
    public string? Name { get; set; }
    public int? SubCategoryId { get; set; }
    public string? OwnerId { get; set; }
}
