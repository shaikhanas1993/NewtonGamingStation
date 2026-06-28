using Microsoft.Extensions.DependencyInjection;
using NewtonGamingStation.Application.Interfaces;
using NewtonGamingStation.Application.Services;

namespace NewtonGamingStation.Application;

/// <summary>
/// Registers application-layer services. Keeping registration close to the layer
/// it configures keeps Program.cs thin and the composition root explicit.
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IGameService, GameService>();
        services.AddScoped<IPublisherService, PublisherService>();
        return services;
    }
}
