using AutoMapper;
using Domain.DTOs.SubCategoryDTOs;
using Domain.Entities;
using Domain.Filters.GetSubCategoryFilter;
using Domain.Responses;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Infrastructure.Services.SubCategoryService;

public class SubCategoryService : ISubCategoryService
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;
    public SubCategoryService(DataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<Response<List<GetSubCategoryDTO>>> GetSubCategory(GetSubCategoryFilter filter)
    {
        var query = _context.SubCategories.AsQueryable();
        if (filter.Name != null)
        {
            query = query.Where(s => s.SubCategoryName.ToLower().Contains(filter.Name.ToLower()));
        }
        if (filter.CategoryId != null)
        {
            query = query.Where(s => s.CategoryId == filter.CategoryId);
        }
        if (filter.ProductType != null)
        {
            query = query.Where(s => s.ProductType == filter.ProductType);
        }
        var subCategories = await query.ToListAsync();
        var mapped = _mapper.Map<List<GetSubCategoryDTO>>(subCategories);
        return new Response<List<GetSubCategoryDTO>>(mapped);
    }
    public async Task<Response<GetSubCategoryDTO>> GetSubCategoryById(int subCategoryId)
    {
        var subCategory = await _context.SubCategories.FirstOrDefaultAsync(s => s.SubCategoryId == subCategoryId);
        if (subCategory == null)
        {
            return new Response<GetSubCategoryDTO>(HttpStatusCode.NotFound, "Subcategory not found");
        }
        var mapped = _mapper.Map<GetSubCategoryDTO>(subCategory);
        return new Response<GetSubCategoryDTO>(mapped);
    }
    public async Task<Response<GetSubCategoryDTO>> AddSubCategory(AddSubCategoryDTO sub)
    {
        if (sub == null)
        {
            return new Response<GetSubCategoryDTO>(HttpStatusCode.NotFound, "Please fill the SubCategory");
        }
        var mapped = _mapper.Map<SubCategory>(sub);
        await _context.SubCategories.AddAsync(mapped);
        await _context.SaveChangesAsync();
        return new Response<GetSubCategoryDTO>(HttpStatusCode.OK, "Subcategory added successfully");
    }
    public async Task<Response<GetSubCategoryDTO>> UpdateSubCategory(AddSubCategoryDTO subCategory)
    {
        if (subCategory == null)
        {
            return new Response<GetSubCategoryDTO>(HttpStatusCode.NotFound, "Please fill parameter");
        }
        var find = await _context.SubCategories.AsNoTracking().FirstOrDefaultAsync(s => s.SubCategoryId == subCategory.SubCategoryId);
        if (find != null)
        {
            var mapped = _mapper.Map<SubCategory>(subCategory);
            _context.SubCategories.Update(mapped);
            await _context.SaveChangesAsync();
            return new Response<GetSubCategoryDTO>(HttpStatusCode.OK, "SubCategory updated successfully");
        }
        return new Response<GetSubCategoryDTO>(HttpStatusCode.NotFound, "SubCategory not found");
    }

    public async Task<Response<GetSubCategoryDTO>> DeleteSubCategory(int subCategoryId)
    {
        var subCategory = await _context.SubCategories.FirstOrDefaultAsync(s => s.SubCategoryId == subCategoryId);
        if (subCategory == null)
        {
            return new Response<GetSubCategoryDTO>(HttpStatusCode.NotFound, "SubCategory not found");
        }
        _context.SubCategories.Remove(subCategory);
        await _context.SaveChangesAsync();
        return new Response<GetSubCategoryDTO>(HttpStatusCode.OK, "SubCategory was deleted successfully");
    }
}
