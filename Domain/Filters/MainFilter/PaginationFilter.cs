using System.ComponentModel.DataAnnotations;

namespace Domain.Filters.MainFilter;

// PageNumber/PageSize feed straight into `(PageNumber - 1) * PageSize` for Skip and PageSize for
// Take across every paginated Get method (all 22 filters deriving from this one). This
// constructor's own clamping never actually runs it - [FromQuery] binding always uses the
// parameterless constructor + property setters, not this one - so a client passing
// pageNumber=0/negative or pageSize=0/negative reached Skip/Take unclamped and either crashed
// with a raw Postgres "OFFSET/LIMIT must not be negative" (500, full stack trace, unhandled) or,
// for pageSize=0, produced a nonsense TotalPage (division by zero -> float.PositiveInfinity ->
// int.MaxValue on the cast), confirmed live on several endpoints. [Range] here is enforced by
// ASP.NET Core's normal model validation after binding, which every controller in this codebase
// already checks via `if (ModelState.IsValid)` - so this closes it everywhere without touching
// any of the ~20 controllers or services that use these filters.
public class PaginationFilter
{
    [Range(1, int.MaxValue, ErrorMessage = "PageNumber must be at least 1.")]
    public int PageNumber { get; set; }

    [Range(1, 200, ErrorMessage = "PageSize must be between 1 and 200.")]
    public int PageSize { get; set; }

    public PaginationFilter()
    {
        PageNumber = 1;
        PageSize = 10;
    }

    public PaginationFilter(int pageNumber, int pageSize)
    {
        PageNumber = pageNumber < 1 ? 1 : pageNumber;
        PageSize = pageSize < 10 ? 10 : pageSize;
    }
}