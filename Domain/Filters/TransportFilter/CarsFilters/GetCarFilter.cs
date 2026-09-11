using Domain.Filters.MainFilter;

namespace Domain.Filters.TransportFilters.CarsFilter;

public class GetCarFilter : PaginationFilter
{
    public string? Model { get; set; }
    public int? SubCategoryId { get; set; }
    public string? OwnerId { get; set; }
}
