using Microsoft.AspNetCore.Http;

namespace Domain.DTOs.KomTechDTOs.NoteBookDTOs;

public class AddNoteBookDTO : NoteBookDTO
{
    public List<IFormFile> Images { get; set; } = null!;
}
