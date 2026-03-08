using ParcelTracking.Domain.Entities;
using ParcelTracking.Domain.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace ParcelTracking.Domain.Interfaces
{
    public interface IParcelEventRepository
    {
        Task AddEventAsync(ParcelEvent parcelEvent);

        Task<List<ParcelEvent>> GetEventsAsync(string trackingId);
    }
}
