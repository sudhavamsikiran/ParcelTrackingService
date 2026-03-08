using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Azure.Cosmos;
using ParcelTracking.Application.Interfaces;
using ParcelTracking.Application.ReadModels;
using ParcelTracking.Infrastructure.Persistence;

namespace ParcelTracking.Infrastructure.Repositories
{
    public class CosmosTrackingQueryRepository : ITrackingQueryRepository
    {
        private readonly Container _container;

        public CosmosTrackingQueryRepository(CosmosDbContext context)
        {
            //_container = context.Database
            //    .CreateContainerIfNotExistsAsync(
            //        "ParcelTrackingView",
            //        "/trackingId")
            //    .GetAwaiter()
            //    .GetResult();
            _container = context.TrackingViewContainer;
        }

        public async Task<ParcelTrackingView> GetAsync(string trackingId)
        {
            try
            {
                var response = await _container.ReadItemAsync<ParcelTrackingView>(
                    trackingId,
                    new PartitionKey(trackingId));

                return response.Resource;
            }
            catch
            {
                return null;
            }
        }

        public async Task UpsertAsync(ParcelTrackingView view)
        {
            await _container.UpsertItemAsync(
                view,
                new PartitionKey(view.TrackingId));
        }
    }
}
