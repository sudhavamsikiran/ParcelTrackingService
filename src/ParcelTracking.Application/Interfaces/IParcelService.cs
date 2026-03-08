using ParcelTracking.Application.Commands;
using ParcelTracking.Application.Dtos;
 
using System;
using System.Collections.Generic;
using System.Text;

namespace ParcelTracking.Application.Interfaces
{
    public interface IParcelService
    {
        Task ProcessScanAsync(SubmitScanCommand command);

        Task<ParcelDto> GetParcelAsync(string trackingId);
    }
}
