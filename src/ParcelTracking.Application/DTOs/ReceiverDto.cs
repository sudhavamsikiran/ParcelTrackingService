using System;
using System.Collections.Generic;
using System.Text;

namespace ParcelTracking.Application.DTOs
{
    public  class ReceiverDto
    {
        public string Name { get; set; }

        public string Phone { get; set; }

        public bool Notify { get; set; }
    }
}
