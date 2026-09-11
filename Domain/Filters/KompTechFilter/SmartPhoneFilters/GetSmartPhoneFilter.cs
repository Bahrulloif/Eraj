using Domain.Filters.MainFilter;

namespace Domain.Filters.KompTechFilters.SmartPhoneFilters;

public class GetSmartPhoneFilter : PaginationFilter
{
    public string? Name { get; set; }
    public int? SubCategoryId { get; set; }
    public string? OwnerId { get; set; }
}
