using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Domain.DTOs.LoginDTOs;
using Domain.DTOs.RegisterDTO;
using Domain.Entities;
using Domain.Responses;
using Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.AccountService;

public class AccountService : IAccountService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly DataContext _context;
    private readonly IConfiguration _configuration;

    public AccountService(
        UserManager<ApplicationUser> userManager,
        DataContext context,
        IConfiguration configuration)
    {
        _userManager = userManager;
        _context = context;
        _configuration = configuration;
    }
    public async Task<Response<string>> Login(LoginDTO login)
    {
        try
        {
            var findUser = await _userManager.FindByNameAsync(login.UserName);
            if (findUser == null)
            {
                return new Response<string>(System.Net.HttpStatusCode.BadRequest, "Username or Password is incorrect");
            }
            var checkPassword = await _userManager.CheckPasswordAsync(findUser, login.Password);
            if (checkPassword)
            {
                return new Response<string>(GenerateJwtToken(findUser));
            }
            else
            {
                return new Response<string>(System.Net.HttpStatusCode.BadRequest, "Username or password in incorrect");
            }
        }
        catch (System.Exception e)
        {
            return new Response<string>(System.Net.HttpStatusCode.InternalServerError, e.Message);
        }

    }


    public async Task<Response<string>> Register(RegisterDTO user)
    {
        var findUser = await _userManager.FindByNameAsync(user.UserName);
        if (findUser != null)
        {
            return new Response<string>(System.Net.HttpStatusCode.BadRequest, "User already exist");
        }
        var newUser = new ApplicationUser()
        {
            PhoneNumber = user.TelephoneNumber,
            UserName = user.UserName
        };
        await _userManager.CreateAsync(newUser, user.Password);
        await _userManager.AddToRoleAsync(newUser, UserRoles.Admin);


        var newProfile = new ProfileUser()
        {
            ApplicationUserId = newUser.Id,
            Name = user.Name,
            Surname = user.Surname
        };
        await _context.Profiles.AddAsync(newProfile);

        return new Response<string>(System.Net.HttpStatusCode.OK, $"{user.Name} registered successfully");
    }




    private string GenerateJwtToken(ApplicationUser user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_configuration["Jwt:key"]!);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[] { new Claim("id", user.UserName!) }),
            Expires = DateTime.UtcNow.AddHours(1),
            Issuer = _configuration["Jwt:Issuer"],
            Audience = _configuration["Jwt:Audience"],
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}


public static class UserRoles
{
    public const string SuperAdmin = "SuperAdmin";
    public const string Admin = "Admin";
    public const string Marketing = "Marketing";
    public const string User = "User";
    public const string Businessman = "Businessman";
    public const string Courier = "Courier";
}