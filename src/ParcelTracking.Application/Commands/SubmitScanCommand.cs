using ParcelTracking.Application.DTOs;
using System.Text.Json;
using System.Text.Json.Serialization;
namespace ParcelTracking.Application.Commands;

public class SubmitScanCommand
{

    //[JsonPropertyName("id")]
    //public string Id => EventId;
    //public string EventId { get; set; }

    //public string TrackingId { get; set; }

    //public string EventType { get; set; }

    //public string LocationId { get; set; }

    //public string ActorId { get; set; }

    //public DateTime EventTimeUtc { get; set; }

    //public int RetryCount { get; set; } = 0;
    public string EventId { get; set; }

    public string TrackingId { get; set; }

    public string EventType { get; set; }

    public string LocationId { get; set; }

    public string ActorId { get; set; }

    public DateTime EventTimeUtc { get; set; }

    public int RetryCount { get; set; }

    public ScanMetadata Metadata { get; set; }

}
