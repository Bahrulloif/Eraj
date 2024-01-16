using Domain.Filters.MainFilter;

namespace Domain.Filters.KompTechFilters.NoteBookFilters;

public class GetNoteBookFilter : PaginationFilter
{
    public string? Name { get; set; }
}
