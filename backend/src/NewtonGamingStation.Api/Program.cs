using Microsoft.EntityFrameworkCore;
using NewtonGamingStation.Api.Middleware;
using NewtonGamingStation.Application;
using NewtonGamingStation.Infrastructure;
using NewtonGamingStation.Infrastructure.Persistence;
using NewtonGamingStation.Infrastructure.Persistence.Seed;

var builder = WebApplication.CreateBuilder(args);

const string CorsPolicy = "AllowFrontend";

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();

// Composition root: each layer registers its own services.
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

// CORS so the Angular dev server / nginx container can call the API.
var allowedOrigins = builder.Configuration
    .GetSection("Cors:AllowedOrigins").Get<string[]>()
    ?? new[] { "http://localhost:4200", "http://localhost:8080" };

builder.Services.AddCors(options =>
    options.AddPolicy(CorsPolicy, policy =>
        policy.WithOrigins(allowedOrigins).AllowAnyHeader().AllowAnyMethod()));

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// Apply migrations and seed data unless explicitly disabled (e.g. during tests).
if (app.Configuration.GetValue("Database:AutoMigrate", true))
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await DatabaseSeeder.MigrateAndSeedAsync(db);
}

app.UseCors(CorsPolicy);
app.MapControllers();
app.MapGet("/health", () => Results.Ok(new { status = "healthy" }));

app.Run();

// Exposed so the integration test project can reference the entry point via WebApplicationFactory.
public partial class Program { }
