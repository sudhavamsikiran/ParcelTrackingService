using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Text;

namespace ParcelTracking.Worker.Metrics
{
    public static class ParcelMetrics
    {
        private static readonly Meter Meter =
            new("ParcelTracking.Metrics");

        public static Counter<int> EventsProcessed =
            Meter.CreateCounter<int>("parcel_events_processed");

        public static Counter<int> RetryEvents =
            Meter.CreateCounter<int>("parcel_retry_events");

        public static Counter<int> DlqEvents =
            Meter.CreateCounter<int>("parcel_dlq_events");
    }
}
