using Domain.Filters.MainFilter;

namespace Domain.Filters.TransportFilter.TruckFilters;

public class TruckFilter : PaginationFilter
{
    public string? Model { get; set; }
}
