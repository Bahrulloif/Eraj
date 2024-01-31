using Domain.DTOs.SubCategoryDTOs;
using Domain.Filters.GetSubCategoryFilter;
using Domain.Responses;

namespace Infrastructure.Services.SubCategoryService;

public interface ISubCategoryService
{
    public Task<Response<List<GetSubCategoryDTO>>> GetSubCategory(GetSubCategoryFilter filter);
    public Task<Response<GetSubCategoryDTO>> GetSubCategoryById(int subCategoryId);
    public Task<Response<GetSubCategoryDTO>> AddSubCategory(AddSubCategoryDTO subCategory);
    public Task<Response<GetSubCategoryDTO>> UpdateSubCategory(AddSubCategoryDTO subCategory);
    public Task<Response<GetSubCategoryDTO>> DeleteSubCategory(int subCategoryId);
}
