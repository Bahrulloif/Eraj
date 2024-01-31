using Domain.DTOs.PictureDTO;
namespace Domain.DTOs.KomTechDTOs.NoteBookDTOs;

public class GetNoteBookDTO : NoteBookDTO
{
    public List<PictureDto> Images { get; set; } = null!;
}
