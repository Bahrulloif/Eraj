using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class BaseController : ControllerBase
{
    [NonAction]
    public List<string> ModelStateErrors() => ModelState.SelectMany(e => e.Value!.Errors.Select(x => x.ErrorMessage)).ToList();

    /// <summary>The authenticated caller's ApplicationUserId (the "sid" claim set at login), or null if absent.</summary>
    protected string? CurrentUserId => User.FindFirst("sid")?.Value;

    /// <summary>True for SuperAdmin/Admin — allowed to act on any user's data, not just their own.</summary>
    protected bool IsPrivilegedUser => User.IsInRole("SuperAdmin") || User.IsInRole("Admin");
}
