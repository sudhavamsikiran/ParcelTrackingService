using System;
using System.Collections.Generic;
using System.Text;

namespace ParcelTracking.Application.DTOs
{
    public class AddressDto
    {
        public string Line1 { get; set; }

        public string Line2 { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string PostalCode { get; set; }
    }
}
