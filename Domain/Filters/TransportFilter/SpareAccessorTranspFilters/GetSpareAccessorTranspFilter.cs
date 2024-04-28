using Domain.Filters.MainFilter;

namespace Domain.Filters.TransportFilter.SpareAccessorTranspFilters;

public class GetSpareAccessorTranspFilter : PaginationFilter
{
    public string? Model { get; set; }
}
