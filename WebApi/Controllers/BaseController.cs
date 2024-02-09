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
}
