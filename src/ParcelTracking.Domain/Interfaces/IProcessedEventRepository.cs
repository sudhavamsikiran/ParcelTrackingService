using System;
using System.Collections.Generic;
using System.Text;

namespace ParcelTracking.Domain.Interfaces
{
    public interface IProcessedEventRepository
    {
        Task<bool> ExistsAsync(string eventId);

        Task MarkProcessedAsync(string eventId);
    }
}
