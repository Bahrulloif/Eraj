using Domain.DTOs.PictureDTO;
namespace Domain.DTOs.KomTechDTOs.NoteBookDTOs;

public class GetNoteBookDTO : NoteBookDTO
{
    public List<PictureDto> Images { get; set; } = null!;
    // Who can edit/delete this listing (Businessman) - was never exposed to any client, making
    // "show me my own listings" impossible to build (see ../../../../../frontend/speca.md, Фаза 6).
    public string? OwnerId { get; set; }
}
