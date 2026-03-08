using ParcelTracking.Domain.Entities;
using ParcelTracking.Domain.Interfaces;

namespace ParcelTracking.Infrastructure.Repositories;

public class InMemoryParcelRepository : IParcelRepository
{
    private static readonly Dictionary<string, Parcel> _storage = new();

    public Task<Parcel> GetAsync(string trackingId)
    {
        _storage.TryGetValue(trackingId, out var parcel);

        return Task.FromResult(parcel);
    }

    public Task CreateAsync(Parcel parcel)
    {
        _storage[parcel.TrackingId] = parcel;

        return Task.CompletedTask;
    }

    public Task UpdateAsync(Parcel parcel)
    {
        _storage[parcel.TrackingId] = parcel;

        return Task.CompletedTask;
    }
}