using System.Reflection;
using AirlineService.Application;
using AirlineService.Infrastructure;
using AirlineService.Infrastructure.Data;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Airline Service API",
        Version = "v1",
        Description = "Web API for managing airline flight statuses. Provides endpoints for viewing, adding, and updating flights."
    });

    var webApiXmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var webApiXmlPath = Path.Combine(AppContext.BaseDirectory, webApiXmlFile);
    if (File.Exists(webApiXmlPath))
    {
        options.IncludeXmlComments(webApiXmlPath);
    }

    var applicationAssembly = typeof(AirlineService.Application.DependencyInjection).Assembly;
    var appXmlFile = $"{applicationAssembly.GetName().Name}.xml";
    var appXmlPath = Path.Combine(AppContext.BaseDirectory, appXmlFile);
    if (File.Exists(appXmlPath))
    {
        options.IncludeXmlComments(appXmlPath);
    }
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var initializer = scope.ServiceProvider.GetRequiredService<ApplicationDbContextInitializer>();
    await initializer.InitializeAsync();
    await initializer.SeedAsync();
}

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Airline Service API v1");
});

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

await app.RunAsync();
