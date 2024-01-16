using Domain.Filters.MainFilter;

namespace Domain.Filters.KompTechFilters.TabletFilter;

public class GetTabletFilter : PaginationFilter
{
    public string? Name { get; set; }
}
