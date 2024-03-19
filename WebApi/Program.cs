using Infrastructure.AutoMapper;
using Infrastructure.Seed;
using WebApi.ExtensionMethods.SwaggerConfiguration;
using WebApi.ExtentionsMethods.AddAuthConfiguraion;
using WebApi.ExtentionsMethods.AddServices;

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
    seed.SeedRoles();
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

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
