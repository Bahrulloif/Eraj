using Domain.DTOs.CatalogDTOs;
using Domain.Filters.CatalogFilter;
using Domain.Responses;

namespace Infrastructure.Services.CatalogService;

public interface ICatalogService
{
    public Task<Response<List<GetCatalogDTO>>> GetCatalog(GetCatalogFilter filter);
    public Task<Response<GetCatalogDTO>> GetCatalogById(int catalogId);
    public Task<Response<GetCatalogDTO>> AddCatalog(AddCatalogDTO catalog);
    public Task<Response<GetCatalogDTO>> UpdateCatalog(AddCatalogDTO catalog);
    public Task<Response<GetCatalogDTO>> DeleteCatalog(int catalogId);
}
