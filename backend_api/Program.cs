using dotnet.Endpoints;
using dotnet.Repositories;
using dotnet.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApiDocument(config =>
{
    // App-level metadata for OpenAPI
    config.Title = "Master Race Management API";
    config.Description = "RESTful API for creating and managing 'master race' entities with business validations.";
    config.Version = "1.0.0";
});

// Dependency Injection
builder.Services.AddSingleton<IMasterRaceRepository, InMemoryMasterRaceRepository>();
builder.Services.AddSingleton<IMasterRaceService, MasterRaceService>();

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.SetIsOriginAllowed(_ => true)
              .AllowCredentials()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Use CORS
app.UseCors("AllowAll");

// Configure OpenAPI/Swagger
app.UseOpenApi();
app.UseSwaggerUi(config =>
{
    config.Path = "/docs";
});

/*
 Health check endpoint
*/
app.MapGet("/", () => new { message = "Healthy" })
   .WithName("HealthCheck");

// Map MasterRace endpoints
app.MapMasterRaceEndpoints();

app.Run();