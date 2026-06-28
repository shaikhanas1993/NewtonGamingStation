using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NewtonGamingStation.Application.Interfaces;
using NewtonGamingStation.Infrastructure.Persistence;
using NewtonGamingStation.Infrastructure.Repositories;

namespace NewtonGamingStation.Infrastructure;

public static class DependencyInjection
{
    /// <summary>
    /// Registers the SQL Server DbContext AND all repository implementations.
    /// Used in production and local development.
    /// </summary>
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException(
                "Connection string 'DefaultConnection' is not configured.");

        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(connectionString, sql =>
            {
                sql.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName);
                sql.EnableRetryOnFailure();
            }));

        services.AddInfrastructureRepositories();
        return services;
    }

    /// <summary>
    /// Registers only the repository implementations (no DbContext).
    /// Called by the integration-test host so the factory can supply its own
    /// SQLite DbContext without any SQL Server dependency being registered.
    /// </summary>
    public static IServiceCollection AddInfrastructureRepositories(
        this IServiceCollection services)
    {
        services.AddScoped<IGameRepository, GameRepository>();
        services.AddScoped<IPublisherRepository, PublisherRepository>();
        return services;
    }
}