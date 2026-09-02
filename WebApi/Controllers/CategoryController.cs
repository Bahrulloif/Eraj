using Domain.DTOs.CategoryDTOs;
using Domain.Responses;
using Infrastructure.Services.CategoryService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoryController : BaseController
{
    private readonly ICategoryService _categoryService;
    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }
    [HttpGet("get/category"), AllowAnonymous]
    public async Task<ActionResult> GetCategory([FromQuery] GetCategoryFilter filter)
    {
        if (ModelState.IsValid)
        {
            var result = await _categoryService.GetCategory(filter);
            return StatusCode(result.StatusCode, result);
        }
        var response = new Response<List<GetCategoryDTO>>(System.Net.HttpStatusCode.BadRequest, ModelStateErrors());
        return StatusCode(response.StatusCode, response);
    }

    [HttpGet("get/categoryById"), AllowAnonymous]
    public async Task<ActionResult> GetCategoryById(int categoryId)
    {
        if (ModelState.IsValid)
        {
            var result = await _categoryService.GetCategoryById(categoryId);
            return StatusCode(result.StatusCode, result);
        }
        var response = new Response<GetCategoryDTO>(System.Net.HttpStatusCode.BadRequest, ModelStateErrors());
        return StatusCode(response.StatusCode, response);
    }

    [HttpPost("post/category")]
    [Authorize(Roles = "SuperAdmin, Admin")]
    public async Task<ActionResult> AddCategory(AddCategoryDTO category)
    {
        if (ModelState.IsValid)
        {
            var result = await _categoryService.AddCategory(category);
            return StatusCode(result.StatusCode, result);
        }
        var response = new Response<GetCategoryDTO>(System.Net.HttpStatusCode.BadRequest, ModelStateErrors());
        return StatusCode(response.StatusCode, response);
    }

    [HttpPut("put/category")]
    [Authorize(Roles = "SuperAdmin, Admin")]
    public async Task<ActionResult> UpdateCategory(AddCategoryDTO category)
    {
        if (ModelState.IsValid)
        {
            var result = await _categoryService.AddCategory(category);
            return StatusCode(result.StatusCode, result);

        }
        var response = new Response<GetCategoryDTO>(System.Net.HttpStatusCode.BadRequest, ModelStateErrors());
        return StatusCode(response.StatusCode, response);
    }

    [HttpDelete("delete/category")]
    [Authorize(Roles = "SuperAdmin, Admin")]
    public async Task<ActionResult> DeleteCategory(int categoryId)
    {
        if (ModelState.IsValid)
        {
            var result = await _categoryService.DeleteCategory(categoryId);
            return StatusCode(result.StatusCode, result);
        }
        var response = new Response<GetCategoryDTO>(System.Net.HttpStatusCode.BadRequest, ModelStateErrors());
        return StatusCode(response.StatusCode, response);
    }
}
