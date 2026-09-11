using Domain.Filters.MainFilter;

namespace Domain.Filters.RealEstateFilters.CommercialRealEstateFilter;

public class GetCommercialRealEstateFilter : PaginationFilter
{
    public int? SubCategoryId { get; set; }
    public string? OwnerId { get; set; }
}
