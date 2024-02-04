using Domain.DTOs.KomTechDTOs.NoteBookDTOs;
using Domain.Filters.KompTechFilters.NoteBookFilters;
using Domain.Responses;

namespace Infrastructure.Services.KompTechService.NoteBookService;

public interface INoteBookService
{
    public Task<PagedResponse<List<GetNoteBookDTO>>> GetNoteBook(GetNoteBookFilter filter);
    public Task<Response<GetNoteBookDTO>> GetNoteBookById(int noteBookId);
    public Task<Response<string>> AddNoteBook(AddNoteBookDTO noteBook);
    public Task<Response<string>> UpdateNoteBook(AddNoteBookDTO noteBook);
    public Task<Response<string>> DeleteNoteBook(int noteBookId);
}
