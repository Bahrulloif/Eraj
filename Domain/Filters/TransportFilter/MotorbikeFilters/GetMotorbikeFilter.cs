using Domain.Filters.MainFilter;

namespace Domain.Filters.TransportFilters.GetMotorbikeFilter;

public class GetMotorbikeFilter : PaginationFilter
{
    public string? Model { get; set; }
}
