using Microsoft.Azure.Cosmos;
using ParcelTracking.Domain.Entities;
using ParcelTracking.Domain.Interfaces;
using ParcelTracking.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Text;

namespace ParcelTracking.Infrastructure.Repositories
{
    public class CosmosParcelRepository : IParcelRepository
    {
        private readonly Container _container;

        public CosmosParcelRepository(CosmosDbContext context)
        {
            _container = context.Database.GetContainer("Parcels");
        }

        public async Task<Parcel> GetAsync(string trackingId)
        {
            try
            {
                var response = await _container.ReadItemAsync<Parcel>(
                    trackingId,
                    new PartitionKey(trackingId));

                return response.Resource;
            }
            catch(CosmosException ex)
            {
                return null;
            }
        }

        public async Task CreateAsync(Parcel parcel)
        {
            parcel.Id = parcel.TrackingId;
            await _container.CreateItemAsync(parcel, new PartitionKey(parcel.TrackingId));
        }

        public async Task UpdateAsync(Parcel parcel)
        {
            await _container.UpsertItemAsync(parcel, new PartitionKey(parcel.TrackingId));
        }
    }
}
