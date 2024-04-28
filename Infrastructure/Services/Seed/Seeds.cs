using Domain.DTOs.LoginDTOs;
using Domain.Entities;
using Domain.Responses;
using Infrastructure.AccountService;
using Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Seed;

public class Seeds
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<Roles> _roleManager;
    private readonly DataContext _context;
    private readonly IConfiguration _configuration;


    public Seeds(UserManager<ApplicationUser> userManager,
        RoleManager<Roles> roleManager,
        DataContext context,
        IConfiguration configuration)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _context = context;
        _configuration = configuration;

    }

    public async Task SeedRoles()
    {
        var listRoles = new List<string> { "SuperAdmin", "Admin", "Marketing", "User", "Businessman", "Courier" };
        foreach (var item in listRoles)
        {
            var role = await _roleManager.FindByNameAsync(item);

            if (role == null)
            {
                var newRole = new Roles()
                {
                    Name = item
                };
                await _roleManager.CreateAsync(newRole);
            }

        }
    }

    public async Task CreateSuperAdmin()
    {
        var find = await _userManager.FindByNameAsync("SuperAdmin");
        if (find == null)
        {
            var superAdmin = new ApplicationUser
            {
                PhoneNumber = "+992901000455",
                UserName = "SuperAdmin"
            };
            await _userManager.CreateAsync(superAdmin, "Maxshop123");
            await _userManager.AddToRoleAsync(superAdmin, "SuperAdmin");

            var newProfile = new ProfileUser
            {
                ApplicationUserId = superAdmin.Id,
                Name = "Admin",
                Surname = "Super"
            };
            await _context.Profiles.AddAsync(newProfile);
            await _context.SaveChangesAsync();
        }
    }

}