using Microsoft.Extensions.DependencyInjection;
using ParcelTracking.Application.Interfaces;
using ParcelTracking.Domain.Interfaces;
using ParcelTracking.Infrastructure.Messaging;
using ParcelTracking.Infrastructure.Persistence;
using ParcelTracking.Infrastructure.Repositories;
 
namespace ParcelTracking.Infrastructure.DependencyInjection;

public static class InfrastructureServiceRegistration
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddSingleton<CosmosDbContext>();

        services.AddScoped<IParcelRepository, CosmosParcelRepository>();

        services.AddScoped<IParcelEventRepository, CosmosParcelEventRepository>();

        services.AddSingleton<IEventPublisher, KafkaEventPublisher>();

        services.AddScoped<ITrackingQueryRepository, CosmosTrackingQueryRepository>();

        return services;
    }
}
