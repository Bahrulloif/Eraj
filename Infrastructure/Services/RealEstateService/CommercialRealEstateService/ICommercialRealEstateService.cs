using Domain.DTOs.RealEstateDTOs.CommercialRealEstateDTOs;
using Domain.Filters.RealEstateFilters.CommercialRealEstateFilter;
using Domain.Responses;

namespace Infrastructure.Services.RealEstateService.CommercialRealEstateService;

public interface ICommercialRealEstateService
{
    public Task<PagedResponse<List<GetCommercialRealEstateDTO>>> GetCommercialRealEstate(GetCommercialRealEstateFilter filter);
    public Task<Response<GetCommercialRealEstateDTO>> GetCommercialRealEstateById(int commercialRealEstateId);
    public Task<Response<string>> AddCommercialRealEstate(AddCommercialRealEstateDTO commercialRealEstate);
    public Task<Response<string>> UpdateCommercialRealEstate(AddCommercialRealEstateDTO commercialRealEstate);
    public Task<Response<string>> DeleteCommercialRealEstate(int commercialRealEstateId);
}
