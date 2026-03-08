using System;
using System.Collections.Generic;
using System.Text;

namespace ParcelTracking.Domain.ValueObjects
{
    public class Address
    {
        public string Line1 { get; }

        public string Line2 { get; }

        public string City { get; }

        public string State { get; }

        public string PostalCode { get; }

        public Address(
            string line1,
            string line2,
            string city,
            string state,
            string postalCode)
        {
            Line1 = line1;
            Line2 = line2;
            City = city;
            State = state;
            PostalCode = postalCode;
        }
    }
}
