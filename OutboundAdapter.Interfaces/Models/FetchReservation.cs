using System;
using System.Collections.Generic;
using System.Text;

namespace OutboundAdapter.Interfaces.Models
{
    public class FetchReservation
    {
        public int HotelId { get; set; }
        public string ExternalReservationId { get; set; }
    }
}
