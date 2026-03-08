using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
namespace ParcelTracking.Domain.Entities
{
    public class ParcelEvent
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        public string EventId { get; set; }

        public string TrackingId { get; set; }
        public string EventType { get; set; }

        public string LocationId { get; set; }
        public string ActorId { get; set; }
        public int RetryCount { get; set; }

        public object Metadata { get; set; }
        public string Status { get; set; }
        public DateTime EventTimeUtc { get; set; }




    }
}
