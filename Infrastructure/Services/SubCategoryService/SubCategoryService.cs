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
        if (filter.Name != null)
        {
            var find = await _context.SubCategories.Where(s => s.SubCategoryName.ToLower().Contains(filter.Name.ToLower())).ToListAsync();
            var result = _mapper.Map<List<GetSubCategoryDTO>>(find);
            return new Response<List<GetSubCategoryDTO>>(result);
        }
        var subCategories = await _context.SubCategories.ToListAsync();
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
        var find =await _context.SubCategories.AsNoTracking().FirstOrDefaultAsync(s => s.CategoryId == subCategory.CategoryId);
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
