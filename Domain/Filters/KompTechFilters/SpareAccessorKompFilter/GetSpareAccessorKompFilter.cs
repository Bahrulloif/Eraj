using Domain.Filters.MainFilter;

namespace Domain.Filters.KompTechFilters.SpareAccessorKompFilter;

public class GetSpareAccessorKompFilter : PaginationFilter
{
    public string? Name { get; set; }
}
