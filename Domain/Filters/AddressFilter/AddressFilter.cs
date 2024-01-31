using Domain.Filters.MainFilter;

namespace Domain.Filters.AddressFilter;

public class AddressFilter : PaginationFilter
{
    public string? Country { get; set; }
}
