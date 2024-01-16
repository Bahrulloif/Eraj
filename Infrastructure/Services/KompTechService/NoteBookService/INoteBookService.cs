using Domain.DTOs.KomTechDTOs.NoteBookDTOs;
using Domain.Filters.KompTechFilters.NoteBookFilters;
using Domain.Responses;

namespace Infrastructure.Services.KompTechService.NoteBookService;

public interface INoteBookService
{
    public Task<Response<List<GetNoteBookDTO>>> GetNoteBook(GetNoteBookFilter filter);
    public Task<Response<GetNoteBookDTO>> GetNoteBookById(int noteBookId);
    public Task<Response<GetNoteBookDTO>> AddNoteBook(AddNoteBookDTO noteBook);
    public Task<Response<GetNoteBookDTO>> UpdateNoteBook(AddNoteBookDTO noteBook);
    public Task<Response<GetNoteBookDTO>> DeleteNoteBook(int noteBookId);
}
