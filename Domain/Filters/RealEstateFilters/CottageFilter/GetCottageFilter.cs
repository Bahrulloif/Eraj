using Domain.Filters.MainFilter;

namespace Domain.Filters.RealEstateFilters.CottageFilter;

public class GetCottageFilter : PaginationFilter
{
    public int? NumberOfRooms { get; set; }
}
