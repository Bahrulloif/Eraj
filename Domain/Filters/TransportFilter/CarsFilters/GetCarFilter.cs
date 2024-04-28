using Domain.Filters.MainFilter;

namespace Domain.Filters.TransportFilters.CarsFilter;

public class GetCarFilter : PaginationFilter
{
    public string? Model { get; set; }
}
