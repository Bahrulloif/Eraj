

using Domain.Filters.MainFilter;

public class GetCategoryFilter : PaginationFilter
{
    public string? Name { get; set; }
    public int? CatalogId { get; set; }
}