using Domain.Filters.MainFilter;

namespace Domain.Filters.CatalogFilter;

public class GetCatalogFilter : PaginationFilter
{
    public string? Name { get; set; }

    public GetCatalogFilter():base(1,15)
    {
        
    }

}
