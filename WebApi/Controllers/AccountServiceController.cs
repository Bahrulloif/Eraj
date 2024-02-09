using Domain.DTOs.LoginDTOs;
using Domain.DTOs.RegisterDTO;
using Domain.Responses;
using Infrastructure.AccountService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountServiceController : BaseController
{
    private readonly IAccountService _accountService;
    public AccountServiceController(IAccountService accountService)
    {
        _accountService = accountService;
    }
    [HttpPost("post/login"), AllowAnonymous]
    public async Task<IActionResult> Login(LoginDTO login)
    {
        if (ModelState.IsValid)
        {
            var result = await _accountService.Login(login);
            return StatusCode(result.StatusCode, result);
        }
        var response = new Response<string>(System.Net.HttpStatusCode.BadRequest, ModelStateErrors());
        return StatusCode(response.StatusCode, response);
    }
    [HttpPost("post/register"), AllowAnonymous]
    public async Task<IActionResult> Register(RegisterDTO user)
    {
        if (ModelState.IsValid)
        {
            var result = await _accountService.Register(user);
            return StatusCode(result.StatusCode, result);
        }
        var response = new Response<string>(System.Net.HttpStatusCode.BadRequest, ModelStateErrors());
        return StatusCode(response.StatusCode, response);
    }
}
