using Infrastructure.AutoMapper;
using Infrastructure.Seed;
using WebApi.ExtensionMethods.SwaggerConfiguration;
using WebApi.ExtentionsMethods.AddAuthConfiguraion;
using WebApi.ExtentionsMethods.AddServices;

// Npgsql (6+) requires DateTime.Kind == Utc for "timestamp with time zone" columns.
// Request bodies deserialize DateTime as Kind=Unspecified by default, so any client
// not sending an explicit UTC offset (e.g. Order.OrderDate, Cart.DateOfPurchase,
// ProfileUser.Dob) would crash on save. Restore the pre-6.0 lenient behavior instead
// of requiring every client/DTO to be UTC-aware.
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.RegisteredServices(builder.Configuration);
builder.Services.SwaggerService();
builder.Services.AddAuthConfigureService(builder.Configuration);
builder.Services.AddAutoMapper(typeof(ServiceProfile));

var app = builder.Build();

try
{
    var serviceProvider = app.Services.CreateScope().ServiceProvider;
    var seed = serviceProvider.GetRequiredService<Seeds>();
    await seed.SeedRoles();
    await seed.CreateSuperAdmin();
}
catch (System.Exception)
{


}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// FileService.CreateFile writes uploaded product images to wwwroot/Images and returns
// just the file name; without this, that folder was never served and every uploaded
// image was a 404 for clients.
app.UseStaticFiles();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
