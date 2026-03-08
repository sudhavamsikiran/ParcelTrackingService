using System;
using System.Collections.Generic;
using System.Text;

namespace ParcelTracking.Infrastructure.Messaging
{
    public static class KafkaTopics
    {
        public const string ParcelScanEvents = "parcel-scan-events";

        public const string ParcelScanRetry = "parcel-scan-events-retry";

        public const string ParcelScanDLQ = "parcel-scan-events-dlq";

        public const string ParcelNotifications = "parcel-notifications";

    }
}
