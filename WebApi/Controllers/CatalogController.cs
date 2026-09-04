using Domain.DTOs.CatalogDTOs;
using Domain.Filters.CatalogFilter;
using Domain.Responses;
using Infrastructure.Services.CatalogService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CatalogController : BaseController
{
    private readonly ICatalogService _catalogService;

    public CatalogController(ICatalogService catalogService)
    {
        _catalogService = catalogService;
    }

    // Public read, matching Category/SubCategory/products beneath it in the same hierarchy -
    // Catalog is the top of Catalog -> Category -> SubCategory -> products, and every other
    // level is already public; there's no reason browsing the top level alone needs an account,
    // let alone SuperAdmin/Admin (mutations below still require it, unchanged).
    [HttpGet("get/catalogs"), AllowAnonymous]
    public async Task<IActionResult> GetCatalog([FromQuery] GetCatalogFilter filter)
    {
        if (ModelState.IsValid)
        {
            var result = await _catalogService.GetCatalog(filter);
            return StatusCode(result.StatusCode, result);
        }
        var response = new Response<List<GetCatalogDTO>>(System.Net.HttpStatusCode.BadRequest, ModelStateErrors());
        return StatusCode(response.StatusCode, response);
    }

    [HttpGet("get/catalogById"), AllowAnonymous]
    public async Task<IActionResult> GetCatalogById(int catalogId)
    {
        if (ModelState.IsValid)
        {
            var result = await _catalogService.GetCatalogById(catalogId);
            return StatusCode(result.StatusCode, result);
        }
        var response = new Response<GetCatalogDTO>(System.Net.HttpStatusCode.BadRequest, ModelStateErrors());
        return StatusCode(response.StatusCode, response);
    }

    [HttpPost("post/catalog")]
    [Authorize(Roles = "SuperAdmin, Admin")]
    public async Task<ActionResult> AddCatalog(AddCatalogDTO catalog)
    {
        if (ModelState.IsValid)
        {
            var result = await _catalogService.AddCatalog(catalog);
            return StatusCode(result.StatusCode, result);
        }
        var response = new Response<GetCatalogDTO>(System.Net.HttpStatusCode.BadRequest, ModelStateErrors());
        return StatusCode(response.StatusCode, response);
    }

    [HttpPut("put/catalog")]
    [Authorize(Roles = "SuperAdmin, Admin")]
    public async Task<ActionResult> UpdateCatalog(AddCatalogDTO catalog)
    {
        if (ModelState.IsValid)
        {
            var result = await _catalogService.UpdateCatalog(catalog);
            return StatusCode(result.StatusCode, result);
        }
        var response = new Response<GetCatalogDTO>(System.Net.HttpStatusCode.BadRequest, ModelStateErrors());
        return StatusCode(response.StatusCode, response);
    }

    [HttpDelete("deleted/catalog")]
    [Authorize(Roles = "SuperAdmin, Admin")]
    public async Task<ActionResult> DeleteCatalog(int catalogId)
    {
        if (ModelState.IsValid)
        {
            var result = await _catalogService.DeleteCatalog(catalogId);
            return StatusCode(result.StatusCode, result);
        }
        var response = new Response<GetCatalogDTO>(System.Net.HttpStatusCode.NotFound, "Not Found");
        return StatusCode(response.StatusCode, response);
    }

}
