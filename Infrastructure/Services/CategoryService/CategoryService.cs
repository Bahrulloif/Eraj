using AutoMapper;
using Domain.DTOs.CategoryDTOs;
using Domain.Entities;
using Domain.Responses;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Infrastructure.Services.CategoryService;

public class CategoryService : ICategoryService
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;
    public CategoryService(DataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<Response<List<GetCategoryDTO>>> GetCategory(GetCategoryFilter filter)
    {
        if (filter.Name != null)
        {
            var find = await _context.Categories.Where(c => c.CategoryName.ToLower().Contains(filter.Name.ToLower())).ToListAsync();
            var result = _mapper.Map<List<GetCategoryDTO>>(find);
            return new Response<List<GetCategoryDTO>>(result);
        }
        var categories = await _context.Categories.ToListAsync();
        var mapped = _mapper.Map<List<GetCategoryDTO>>(categories);
        return new Response<List<GetCategoryDTO>>(mapped);
    }
    public async Task<Response<GetCategoryDTO>> GetCategoryById(int categoryId)
    {
        var category = await _context.Categories.FirstAsync(c => c.CategoryId == categoryId);
        var result = _mapper.Map<GetCategoryDTO>(category);
        return new Response<GetCategoryDTO>(result);
    }
    public async Task<Response<GetCategoryDTO>> AddCategory(AddCategoryDTO category)
    {
        if (category != null)
        {
            var mapped = _mapper.Map<Category>(category);
            await _context.Categories.AddAsync(mapped);
            await _context.SaveChangesAsync();
            return new Response<GetCategoryDTO>(HttpStatusCode.OK, "Category added successfully");
        }

        return new Response<GetCategoryDTO>(HttpStatusCode.NotFound, "Please fill the category");
    }
    public async Task<Response<GetCategoryDTO>> UpdateCategory(AddCategoryDTO category)
    {
        if (category == null)
        {
            return new Response<GetCategoryDTO>(HttpStatusCode.NotFound, "Please fill the category");
        }
        var query = await _context.Categories.AsNoTracking().FirstOrDefaultAsync(c => c.CategoryId == category.CategoryId);
        if (query == null)
        {
            return new Response<GetCategoryDTO>(HttpStatusCode.NotFound, "Category not found");
        }
        var mapped = _mapper.Map<Category>(category);
        _context.Categories.Update(mapped);
        await _context.SaveChangesAsync();
        return new Response<GetCategoryDTO>(HttpStatusCode.OK, "Category updated successfully");
    }
    public async Task<Response<GetCategoryDTO>> DeleteCategory(int categoryId)
    {
        var find = await _context.Categories.FirstOrDefaultAsync(c => c.CategoryId == categoryId);
        if (find == null)
        {
            return new Response<GetCategoryDTO>(HttpStatusCode.NotFound, "Category not found");
        }
        _context.Categories.Remove(find);
        await _context.SaveChangesAsync();
        return new Response<GetCategoryDTO>(HttpStatusCode.OK, "Category deleted successfully");
    }

}
