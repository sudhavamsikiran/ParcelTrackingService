using System;
using System.Collections.Generic;
using System.Text;

namespace ParcelTracking.Domain.Entities
{
    public class ParcelNotificationEvent
    {
        public string TrackingId { get; set; }

        public string Status { get; set; }

        public string LocationId { get; set; }

        public DateTime EventTimeUtc { get; set; }
    }
}
