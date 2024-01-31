using Domain.Filters.MainFilter;

namespace Domain.Filters.DeliveryAddressFilter;

public class DeliveryAddressFilter : PaginationFilter
{
    public int? Id { get; set; }
}
