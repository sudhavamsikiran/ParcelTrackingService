using ParcelTracking.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ParcelTracking.Domain.Interfaces
{
    public interface IParcelRepository
    {
        Task<Parcel> GetAsync(string trackingId);

        Task CreateAsync(Parcel parcel);

        Task UpdateAsync(Parcel parcel);
    }
}
