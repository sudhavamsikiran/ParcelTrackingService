namespace ParcelTracking.Domain.Entities;

public class ProcessedEvent
{
    public string Id { get; set; }

    public string EventId { get; set; }

    public DateTime ProcessedAt { get; set; }
}