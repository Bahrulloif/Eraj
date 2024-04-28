using Domain.Filters.MainFilter;

namespace Domain.Filters.RoleFilter;

public class GetRoleFilter : PaginationFilter
{
    public string? RoleName { get; set; }
}
