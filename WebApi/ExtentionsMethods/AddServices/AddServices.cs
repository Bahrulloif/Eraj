using AutoMapper;
using Domain.Entities;
using Infrastructure.AccountService;
using Infrastructure.Data;
using Infrastructure.Services.AddressService;
using Infrastructure.Services.CartService;
using Infrastructure.Services.CatalogService;
using Infrastructure.Services.CategoryService;
using Infrastructure.Services.DeliveryAddressService;
using Infrastructure.Services.FileService;
using Infrastructure.Services.KompTechService.NoteBookService;
using Infrastructure.Services.KompTechService.SmartPhoneService;
using Infrastructure.Services.KompTechService.TabletService;
using Infrastructure.Services.OrderService;
using Infrastructure.Services.ProfileService;
using Infrastructure.Services.RecommendationService;
using Infrastructure.Services.SubCategoryService;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace WebApi.ExtentionsMethods.AddServices;

public static class AddServices
{
    public static void RegisteredServices(this IServiceCollection services, IConfiguration configuration)
    {
        // services.AddScoped<IMapper>();
        services.AddScoped<ICatalogService, CatalogService>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<ISubCategoryService, SubCategoryService>();
        services.AddScoped<INoteBookService, NoteBookService>();
        services.AddScoped<ISmartPhoneService, SmartPhoneService>();
        services.AddScoped<ITabletService, TabletService>();
        services.AddScoped<IRecommendationService, RecommendationService>();
        services.AddScoped<IAccountService, AccountService>();
        services.AddScoped<IOrderService, OrderService>();
        services.AddScoped<ICartService, CartService>();
        services.AddScoped<IAddressService, AddressService>();
        services.AddScoped<IDeliveryAddressService, DeliveryAddressService>();
        services.AddScoped<IProfileService, ProfileService>();
        services.AddScoped<IFileService, FileService>();

        services.AddIdentity<ApplicationUser, IdentityRole>(config =>
            {
                config.Password.RequiredLength = 4;
                config.Password.RequireDigit = false; // must have at least one digit
                config.Password.RequireNonAlphanumeric = false; // must have at least one non-alphanumeric character
                config.Password.RequireUppercase = false; // must have at least one uppercase character
                config.Password.RequireLowercase = false;  // must have at least one lowercase character
            })
            //for registering userManager and signinManager
            .AddEntityFrameworkStores<DataContext>()
            .AddDefaultTokenProviders();

        var defaultConnection = services.AddDbContext<DataContext>(configure => configure.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
    }
}
