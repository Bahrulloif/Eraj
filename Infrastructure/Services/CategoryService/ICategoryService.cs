using Domain.DTOs.CategoryDTOs;
using Domain.Responses;

namespace Infrastructure.Services.CategoryService;

public interface ICategoryService
{
    public Task<Response<List<GetCategoryDTO>>> GetCategory(GetCategoryFilter filter);
    public Task<Response<GetCategoryDTO>> GetCategoryById(int categoryId);
    public Task<Response<GetCategoryDTO>> AddCategory(AddCategoryDTO category);
    public Task<Response<GetCategoryDTO>> UpdateCategory(AddCategoryDTO category);
    public Task<Response<GetCategoryDTO>> DeleteCategory(int categoryId);
}
