using Domain.Filters.MainFilter;

namespace Domain.Filters.OrderFilter;

public class OrderFilter : PaginationFilter
{
    public int? Id { get; set; }
}
