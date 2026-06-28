using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using NewtonGamingStation.Infrastructure.Persistence;
using NewtonGamingStation.Infrastructure.Persistence.Seed;

namespace NewtonGamingStation.IntegrationTests;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    private readonly SqliteConnection _connection = new("DataSource=:memory:");

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");
        builder.UseSetting("Database:AutoMigrate", "false");

        builder.ConfigureServices(services =>
        {
            _connection.Open();

            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlite(_connection));

            using var scope = services.BuildServiceProvider().CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            db.Database.EnsureCreated();
            DatabaseSeeder.SeedAsync(db).GetAwaiter().GetResult();
        });
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        if (disposing) _connection.Dispose();
    }
}