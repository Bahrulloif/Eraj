using Domain.Filters.MainFilter;

namespace Domain.Filters.KompTechFilters.NoteBookFilters;

public class GetNoteBookFilter : PaginationFilter
{
    public string? Name { get; set; }
    public int? SubCategoryId { get; set; }
    public string? OwnerId { get; set; }
}
