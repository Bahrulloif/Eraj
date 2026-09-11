using Domain.Filters.MainFilter;

namespace Domain.Filters.TransportFilters.GetMotorbikeFilter;

public class GetMotorbikeFilter : PaginationFilter
{
    public string? Model { get; set; }
    public int? SubCategoryId { get; set; }
    public string? OwnerId { get; set; }
}
