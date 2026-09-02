using Domain.DTOs.KomTechDTOs.NoteBookDTOs;
using Domain.Filters.KompTechFilters.NoteBookFilters;
using Domain.Responses;

namespace Infrastructure.Services.KompTechService.NoteBookService;

public interface INoteBookService
{
    public Task<PagedResponse<List<GetNoteBookDTO>>> GetNoteBook(GetNoteBookFilter filter);
    public Task<Response<GetNoteBookDTO>> GetNoteBookById(int noteBookId);
    public Task<Response<string>> AddNoteBook(AddNoteBookDTO noteBook, string currentUserId);
    public Task<Response<string>> UpdateNoteBook(AddNoteBookDTO noteBook, string currentUserId, bool isPrivileged);
    public Task<Response<string>> DeleteNoteBook(int noteBookId, string currentUserId, bool isPrivileged);
}
