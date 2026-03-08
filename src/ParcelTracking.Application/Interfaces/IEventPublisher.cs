using System;
using System.Collections.Generic;
using System.Text;

namespace ParcelTracking.Application.Interfaces
{
    public interface IEventPublisher
    {
        Task PublishAsync(string topic, string key, object message);
    }
}
