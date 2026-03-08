using Microsoft.Azure.Cosmos;
using ParcelTracking.Domain.Entities;
using ParcelTracking.Domain.Interfaces;
using ParcelTracking.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Text;

namespace ParcelTracking.Infrastructure.Repositories
{
    public class CosmosParcelEventRepository : IParcelEventRepository
    {
        private readonly Container _container;

        public CosmosParcelEventRepository(CosmosDbContext context)
        {
            _container = context.ParcelEventsContainer;
        }

        public async Task AddEventAsync(ParcelEvent parcelEvent)
        {
            await _container.CreateItemAsync(
                parcelEvent,
                new PartitionKey(parcelEvent.TrackingId));
        }

        public async Task<List<ParcelEvent>> GetEventsAsync(string trackingId)
        {
            var query = _container.GetItemQueryIterator<ParcelEvent>(
                new QueryDefinition(
                    "SELECT * FROM c WHERE c.trackingId = @trackingId ORDER BY c.eventTimeUtc DESC")
                    .WithParameter("@trackingId", trackingId));

            var results = new List<ParcelEvent>();

            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();
                results.AddRange(response);
            }

            return results;
        }
    }
}
