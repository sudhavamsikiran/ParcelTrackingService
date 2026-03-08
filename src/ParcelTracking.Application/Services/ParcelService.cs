using ParcelTracking.Application.Commands;
using ParcelTracking.Application.Dtos;
using ParcelTracking.Application.Interfaces;
using ParcelTracking.Domain.Entities;
using ParcelTracking.Domain.Enums;
using ParcelTracking.Domain.Interfaces;
using ParcelTracking.Domain.ValueObjects;

namespace ParcelTracking.Application.Services;

public class ParcelService : IParcelService
{
    private readonly IParcelRepository _parcelRepository;
    private readonly IEventPublisher _eventPublisher;

    public ParcelService(
        IParcelRepository parcelRepository,
        IEventPublisher eventPublisher)
    {
        _parcelRepository = parcelRepository;
        _eventPublisher = eventPublisher;
    }

    public async Task ProcessScanAsync(SubmitScanCommand command)
    {
        var parcel = await _parcelRepository.GetAsync(command.TrackingId);

        if (parcel == null)
        {
            var fromAddress = new Address(
                command.Metadata.FromAddress.Line1,
                command.Metadata.FromAddress.Line2,
                command.Metadata.FromAddress.City,
                command.Metadata.FromAddress.State,
                command.Metadata.FromAddress.PostalCode
            );

            var toAddress = new Address(
                command.Metadata.ToAddress.Line1,
                command.Metadata.ToAddress.Line2,
                command.Metadata.ToAddress.City,
                command.Metadata.ToAddress.State,
                command.Metadata.ToAddress.PostalCode
            );

            var dimensions = new ParcelDimensions(
                command.Metadata.Dimensions.Length,
                command.Metadata.Dimensions.Width,
                command.Metadata.Dimensions.Height,
                command.Metadata.Dimensions.Weight
            );

            parcel = new Parcel(
                command.TrackingId,
                fromAddress,
                toAddress,
                dimensions,
                100,
                command.Metadata.Sender.Phone,
                command.Metadata.Receiver.Phone,
                command.Metadata.Receiver.Notify
            );

            await _parcelRepository.CreateAsync(parcel);
        }

        var newStatus = Enum.Parse<ParcelStatus>(command.EventType);

        parcel.UpdateStatus(newStatus);

        await _parcelRepository.UpdateAsync(parcel);

        await _eventPublisher.PublishAsync(
            "parcel-scan-events",
            command.TrackingId,
            command);
    }

    public async Task<ParcelDto?> GetParcelAsync(string trackingId)
    {
        var parcel = await _parcelRepository.GetAsync(trackingId);

        if (parcel == null)
            return null;

        return new ParcelDto
        {
            TrackingId = parcel.TrackingId,
            CurrentStatus = parcel.CurrentStatus.ToString(),
            SizeClass = parcel.SizeClass,
            BaseCharge = parcel.BaseCharge,
            Surcharge = parcel.Surcharge
        };
    }
}