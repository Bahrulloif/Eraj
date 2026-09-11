using Domain.Filters.MainFilter;

namespace Domain.Filters.TransportFilter.SpareAccessorTranspFilters;

public class GetSpareAccessorTranspFilter : PaginationFilter
{
    public string? Model { get; set; }
    public int? SubCategoryId { get; set; }
    public string? OwnerId { get; set; }
}
