using System;
using System.Collections.Generic;
using System.Text;

namespace ParcelTracking.Application.DTOs
{
    public class ScanMetadata
    {
        public double Weight { get; set; }

        public ParcelDimensionsDto Dimensions { get; set; }

        public SenderDto Sender { get; set; }

        public ReceiverDto Receiver { get; set; }

        public AddressDto FromAddress { get; set; }

        public AddressDto ToAddress { get; set; }
    }
}
