using Domain.DTOs.KomTechDTOs.NoteBookDTOs;
using Domain.Filters.KompTechFilters.NoteBookFilters;
using Domain.Responses;
using Infrastructure.Services.KompTechService.NoteBookService;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.KompTechController;

[ApiController]
[Route("api/[controller]")]
public class NoteBookController : BaseController
{
    private readonly INoteBookService _noteBookService;
    public NoteBookController(INoteBookService noteBookService)
    {
        _noteBookService = noteBookService;
    }

    [HttpGet("get/notebook")]
    public async Task<IActionResult> GetNoteBook([FromQuery] GetNoteBookFilter filter)
    {
        if (ModelState.IsValid)
        {
            var result = await _noteBookService.GetNoteBook(filter);
            return StatusCode(result.StatusCode, result);
        }
        var response = new Response<List<GetNoteBookDTO>>(System.Net.HttpStatusCode.BadRequest, ModelStateErrors());
        return StatusCode(response.StatusCode, response);
    }

    [HttpGet("get/notebookById")]
    public async Task<IActionResult> GetNoteBookById(int noteBookId)
    {
        if (ModelState.IsValid)
        {
            var result = await _noteBookService.GetNoteBookById(noteBookId);
            return StatusCode(result.StatusCode, result);
        }
        var response = new Response<GetNoteBookDTO>(System.Net.HttpStatusCode.BadRequest, ModelStateErrors());
        return StatusCode(response.StatusCode, response);
    }

    [HttpPost("post/notebook")]
    public async Task<IActionResult> AddNoteBook([FromQuery] AddNoteBookDTO noteBook)
    {
        if (ModelState.IsValid)
        {
            var result = await _noteBookService.AddNoteBook(noteBook);
            return StatusCode(result.StatusCode, result);
        }
        var response = new Response<GetNoteBookDTO>(System.Net.HttpStatusCode.BadRequest, ModelStateErrors());
        return StatusCode(response.StatusCode, response);
    }

    [HttpPut("put/notebook")]
    public async Task<IActionResult> UpdateNoteBook(AddNoteBookDTO noteBook)
    {
        if (ModelState.IsValid)
        {
            var result = await _noteBookService.UpdateNoteBook(noteBook);
            return StatusCode(result.StatusCode, result);
        }
        var response = new Response<GetNoteBookDTO>(System.Net.HttpStatusCode.BadRequest, ModelStateErrors());
        return StatusCode(response.StatusCode, response);
    }

    [HttpDelete("delete/notebook")]
    public async Task<IActionResult> DeleteNoteBook(int noteBookId)
    {
        if (ModelState.IsValid)
        {
            var result = await _noteBookService.DeleteNoteBook(noteBookId);
            return StatusCode(result.StatusCode, result);
        }
        var response = new Response<GetNoteBookDTO>(System.Net.HttpStatusCode.BadRequest, ModelStateErrors());
        return StatusCode(response.StatusCode, response);
    }
}
