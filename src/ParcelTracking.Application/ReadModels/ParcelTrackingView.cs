using System;
using System.Collections.Generic;
using System.Text;

namespace ParcelTracking.Application.ReadModels
{
    public class ParcelTrackingView
    {
        public string Id { get; set; }

        public string TrackingId { get; set; }

        public string CurrentStatus { get; set; }

        public string LastLocation { get; set; }

        public DateTime LastUpdated { get; set; }

        public List<TrackingEventView> Events { get; set; } = new();
    }

    public class TrackingEventView
    {
        public string Status { get; set; }

        public string Location { get; set; }

        public DateTime EventTime { get; set; }
    }
}
