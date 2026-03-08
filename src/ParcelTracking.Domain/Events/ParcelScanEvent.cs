using System;
using System.Collections.Generic;
using System.Text;

namespace ParcelTracking.Domain.Events
{
    public class ParcelScanEvent
    {
        public string EventId { get; set; }

        public string TrackingId { get; set; }

        public string EventType { get; set; }

        public DateTime EventTimeUtc { get; set; }

        public string LocationId { get; set; }

        public string ActorId { get; set; }

        public Dictionary<string, string> Metadata { get; set; }
    }
}
