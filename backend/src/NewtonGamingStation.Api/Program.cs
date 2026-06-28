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

builder.Services.AddApplication();

// In "Testing", the factory supplies its own SQLite DbContext — registering
// SQL Server here would cause a dual-provider conflict. Repositories are still
// needed, so only those are registered. Every other environment gets the full stack.
if (builder.Environment.IsEnvironment("Testing"))
{
    builder.Services.AddInfrastructureRepositories();
}
else
{
    builder.Services.AddInfrastructure(builder.Configuration);
}

var allowedOrigins = builder.Configuration
    .GetSection("Cors:AllowedOrigins").Get<string[]>()
    ?? new[] { "http://localhost:4200", "http://localhost:8080" };

builder.Services.AddCors(options =>
    options.AddPolicy(CorsPolicy, policy =>
        policy.WithOrigins(allowedOrigins).AllowAnyHeader().AllowAnyMethod()));

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
    app.MapOpenApi();

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

public partial class Program { }