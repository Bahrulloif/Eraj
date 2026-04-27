using Domain.Filters.MainFilter;

namespace Domain.Filters.KompTechFilters.SmartPhoneFilters;

public class GetSmartPhoneFilter : PaginationFilter
{
    public string? Name { get; set; }
}
