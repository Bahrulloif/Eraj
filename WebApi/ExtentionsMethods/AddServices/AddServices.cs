using AutoMapper;
using Infrastructure.AccountService;
using Infrastructure.Data;
using Infrastructure.Services.CartService;
using Infrastructure.Services.CatalogService;
using Infrastructure.Services.CategoryService;
using Infrastructure.Services.KompTechService.NoteBookService;
using Infrastructure.Services.KompTechService.SmartPhoneService;
using Infrastructure.Services.KompTechService.TabletService;
using Infrastructure.Services.OrderService;
using Infrastructure.Services.RecommendationService;
using Infrastructure.Services.SubCategoryService;
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

        var defaultConnection = services.AddDbContext<DataContext>(configure => configure.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
    }
}
