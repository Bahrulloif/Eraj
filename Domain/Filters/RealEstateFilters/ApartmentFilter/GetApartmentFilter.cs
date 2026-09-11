using Domain.Filters.MainFilter;

namespace Domain.Filters.RealEstateFilters.ApartmentFilter;

public class GetApartmentFilter : PaginationFilter
{
    public int? NumberOfRooms { get; set; }
    public int? SubCategoryId { get; set; }
    public string? OwnerId { get; set; }
}
