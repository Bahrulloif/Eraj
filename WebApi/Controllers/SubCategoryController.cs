using Domain.DTOs.SubCategoryDTOs;
using Domain.Filters.GetSubCategoryFilter;
using Domain.Responses;
using Infrastructure.Services.SubCategoryService;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SubCategoryController : BaseController
{
    private readonly ISubCategoryService _subCategoryService;
    public SubCategoryController(ISubCategoryService subCategoryService)
    {
        _subCategoryService = subCategoryService;
    }
    [HttpGet("get/subcategory")]
    public async Task<ActionResult> GetSubCategory([FromQuery] GetSubCategoryFilter filter)
    {
        if (ModelState.IsValid)
        {
            var result = await _subCategoryService.GetSubCategory(filter);
            return StatusCode(result.StatusCode, result);
        }
        var response = new Response<List<GetSubCategoryDTO>>(System.Net.HttpStatusCode.BadRequest, ModelStateErrors());
        return StatusCode(response.StatusCode, response);
    }

    [HttpGet("get/subcategoryById")]
    public async Task<ActionResult> GetSubCategoryById(int subCategoryId)
    {
        if (ModelState.IsValid)
        {
            var result = await _subCategoryService.GetSubCategoryById(subCategoryId);
            return StatusCode(result.StatusCode, result);
        }
        var response = new Response<GetSubCategoryDTO>(System.Net.HttpStatusCode.BadRequest, ModelStateErrors());
        return StatusCode(response.StatusCode, response);
    }

    [HttpPost("post/subcategory")]
    public async Task<ActionResult> AddSubCategory(AddSubCategoryDTO subCategory)
    {
        if (ModelState.IsValid)
        {
            var result = await _subCategoryService.AddSubCategory(subCategory);
            return StatusCode(result.StatusCode, result);
        }
        var response = new Response<GetSubCategoryDTO>(System.Net.HttpStatusCode.BadRequest, ModelStateErrors());
        return StatusCode(response.StatusCode, response);
    }

    [HttpPut("put/subcategory")]
    public async Task<ActionResult> UpdateSubCategory(AddSubCategoryDTO subCategory)
    {
        if (ModelState.IsValid)
        {
            var result = await _subCategoryService.UpdateSubCategory(subCategory);
            return StatusCode(result.StatusCode, result);
        }
        var response = new Response<GetSubCategoryDTO>(System.Net.HttpStatusCode.BadRequest, ModelStateErrors());
        return StatusCode(response.StatusCode, response);
    }

    [HttpDelete("delete/subcategory")]
    public async Task<ActionResult> DeleteSubCategory(int subCategoryId)
    {
        if (ModelState.IsValid)
        {
            var result = await _subCategoryService.DeleteSubCategory(subCategoryId);
            return StatusCode(result.StatusCode, result);
        }
        var response = new Response<GetSubCategoryDTO>(System.Net.HttpStatusCode.BadRequest, ModelStateErrors());
        return StatusCode(response.StatusCode, response);
    }
}
