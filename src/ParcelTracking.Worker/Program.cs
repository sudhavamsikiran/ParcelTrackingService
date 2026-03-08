using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenTelemetry.Metrics;
using ParcelTracking.Application.Interfaces;
using ParcelTracking.Domain.Interfaces;
using ParcelTracking.Infrastructure.Messaging;
using ParcelTracking.Infrastructure.Persistence;
using ParcelTracking.Infrastructure.Repositories;
using ParcelTracking.Worker.Consumers;



var builder = Host.CreateApplicationBuilder(args);


// --------------------------------------------------
// CONFIGURATION
// --------------------------------------------------

var configuration = builder.Configuration;


// --------------------------------------------------
// COSMOS DB
// --------------------------------------------------

builder.Services.AddSingleton<CosmosDbContext>();


// --------------------------------------------------
// REPOSITORIES
// --------------------------------------------------

builder.Services.AddScoped<IParcelRepository, CosmosParcelRepository>();

builder.Services.AddScoped<IParcelEventRepository, CosmosParcelEventRepository>();

builder.Services.AddScoped<IProcessedEventRepository, CosmosProcessedEventRepository>();

builder.Services.AddScoped<ITrackingQueryRepository, CosmosTrackingQueryRepository>();


// --------------------------------------------------
// EVENT PUBLISHER (Kafka)
// --------------------------------------------------

builder.Services.AddSingleton<IEventPublisher, KafkaEventPublisher>();


// --------------------------------------------------
// KAFKA CONSUMERS
// --------------------------------------------------

builder.Services.AddSingleton<ParcelScanConsumer>();

builder.Services.AddSingleton<ParcelRetryConsumer>();


// --------------------------------------------------
// METRICS (optional for Phase 13)
// --------------------------------------------------

builder.Services.AddOpenTelemetry()
    .WithMetrics(metrics =>
    {
        metrics.AddRuntimeInstrumentation();
    });


// --------------------------------------------------
// BUILD HOST
// --------------------------------------------------

var host = builder.Build();


// --------------------------------------------------
// START WORKERS
// --------------------------------------------------

var scanConsumer = host.Services.GetRequiredService<ParcelScanConsumer>();

var retryConsumer = host.Services.GetRequiredService<ParcelRetryConsumer>();


// Start main Kafka consumer

Task.Run(() => scanConsumer.StartAsync());


// Start retry topic consumer

Task.Run(() => retryConsumer.StartAsync());


// --------------------------------------------------

await host.RunAsync();