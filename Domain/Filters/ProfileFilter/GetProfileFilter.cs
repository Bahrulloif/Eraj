using Domain.Filters.MainFilter;

namespace Domain.Filters.ProfileFilter;

public class GetProfileFilter: PaginationFilter
{
    public string? Name { get; set; }
}
