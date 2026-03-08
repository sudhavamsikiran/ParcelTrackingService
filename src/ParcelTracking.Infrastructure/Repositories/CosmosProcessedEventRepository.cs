using Microsoft.Azure.Cosmos;
using ParcelTracking.Domain.Entities;
using ParcelTracking.Domain.Interfaces;
using ParcelTracking.Infrastructure.Persistence;

namespace ParcelTracking.Infrastructure.Repositories;

public class CosmosProcessedEventRepository : IProcessedEventRepository
{
    private readonly Container _container;

    public CosmosProcessedEventRepository(CosmosDbContext context)
    {
        _container = context.TrackingViewContainer;
    }

    public async Task<bool> ExistsAsync(string eventId)
    {
        try
        {
            var response = await _container.ReadItemAsync<ProcessedEvent>(
                eventId,
                new PartitionKey(eventId));

            return response.Resource != null;
        }
        catch
        {
            return false;
        }
    }

    public async Task MarkProcessedAsync(string eventId)
    {
        var item = new ProcessedEvent
        {
            Id = eventId,
            EventId = eventId,
            ProcessedAt = DateTime.UtcNow
        };

        await _container.UpsertItemAsync(
            item,
            new PartitionKey(eventId));
    }
}