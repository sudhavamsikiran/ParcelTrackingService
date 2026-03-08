using ParcelTracking.Domain.Enums;
using ParcelTracking.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
namespace ParcelTracking.Domain.Entities;

public class Parcel
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    public string TrackingId { get; set; }

    public Address FromAddress { get; set; }

    public Address DestinationAddress { get; set; }

    public string SenderPhone { get; set; }

    public string ReceiverPhone { get; set; }

    public bool ReceiverOptIn { get; set; }

    public ParcelDimensions Dimensions { get; set; }

    public ParcelStatus CurrentStatus { get; set; }

    public string SizeClass { get; set; }

    public decimal BaseCharge { get; set; }

    public decimal Surcharge { get; set; }

    public DateTime CreatedUtc { get; set; }


    // REQUIRED by CosmosDB
    public Parcel()
    {
    }

    // Domain constructor
    public Parcel(
        string trackingId,
        Address from,
        Address destination,
        ParcelDimensions dimensions,
        decimal baseCharge,
        string senderPhone,
        string receiverPhone,
        bool receiverOptIn)
    {
        Initialize(
            trackingId,
            from,
            destination,
            dimensions,
            baseCharge,
            senderPhone,
            receiverPhone,
            receiverOptIn);
    }

    // Shared initialization logic
    private void Initialize(
        string trackingId,
        Address from,
        Address destination,
        ParcelDimensions dimensions,
        decimal baseCharge,
        string senderPhone,
        string receiverPhone,
        bool receiverOptIn)
    {
        Id = trackingId??throw new ArgumentNullException(nameof(trackingId));
        TrackingId = trackingId;

        FromAddress = from;
        DestinationAddress = destination;

        Dimensions = dimensions;

        SenderPhone = senderPhone;
        ReceiverPhone = receiverPhone;
        ReceiverOptIn = receiverOptIn;

        BaseCharge = baseCharge;

        CurrentStatus = ParcelStatus.COLLECTED;
        CreatedUtc = DateTime.UtcNow;

        CalculateParcelSize();
    }

    private void CalculateParcelSize()
    {
        if (Dimensions == null)
        {
            SizeClass = "UNKNOWN";
            Surcharge = 0;
            return;
        }

        if (Dimensions.Length > 50 ||
            Dimensions.Width > 50 ||
            Dimensions.Height > 50)
        {
            SizeClass = "LARGE";
            Surcharge = BaseCharge * 0.20m;
        }
        else
        {
            SizeClass = "NORMAL";
            Surcharge = 0;
        }
    }

    public void UpdateStatus(ParcelStatus newStatus)
    {
        if(CurrentStatus == newStatus) return;
    }
}
 