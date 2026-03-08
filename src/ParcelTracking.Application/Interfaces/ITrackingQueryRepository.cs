using ParcelTracking.Application.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;
 

namespace ParcelTracking.Application.Interfaces
{
    public interface ITrackingQueryRepository
    {
        Task<ParcelTrackingView> GetAsync(string trackingId);

        Task UpsertAsync(ParcelTrackingView view);
    }
}
