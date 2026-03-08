namespace ParcelTracking.Application.Dtos;

public class ParcelDto
{
    public string TrackingId { get; set; }

    public string CurrentStatus { get; set; }

    public string SizeClass { get; set; }

    public decimal BaseCharge { get; set; }

    public decimal Surcharge { get; set; }
    public decimal TotalCharge { get; set; }

}
