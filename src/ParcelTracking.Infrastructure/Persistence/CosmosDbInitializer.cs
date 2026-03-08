using Microsoft.Azure.Cosmos;
using ParcelTracking.Infrastructure.Configuration;
using Microsoft.Extensions.Options;

namespace ParcelTracking.Infrastructure.Persistence;

public class CosmosDbInitializer
{
    private readonly CosmosClient _client;

    private readonly CosmosDbOptions _options;

    private readonly CosmosDbContext _context;

    public CosmosDbInitializer(
        CosmosDbContext context,
        IOptions<CosmosDbOptions> options)
    {
        _context = context;
        _options = options.Value;
        _client = context.Client;
    }

    public async Task InitializeAsync()
    {
        var database =
            await _client.CreateDatabaseIfNotExistsAsync(
                _options.DatabaseName);

        var parcels =
            await database.Database.CreateContainerIfNotExistsAsync(
                "Parcels",
                "/trackingId");

        var events =
            await database.Database.CreateContainerIfNotExistsAsync(
                "ParcelEvents",
                "/trackingId");

        var tracking =
            await database.Database.CreateContainerIfNotExistsAsync(
                "ParcelTrackingView",
                "/trackingId");

        var processed =
            await database.Database.CreateContainerIfNotExistsAsync(
                "ProcessedEvents",
                "/eventId");

        _context.ParcelContainer = parcels.Container;

        _context.ParcelEventsContainer = events.Container;

        _context.TrackingViewContainer = tracking.Container;

        _context.ProcessedEventsContainer = processed.Container;
    }
}