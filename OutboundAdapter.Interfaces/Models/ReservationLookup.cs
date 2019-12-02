using System;
using System.Collections.Generic;
using System.Text;

namespace OutboundAdapter.Interfaces.Models
{
    public class ReservationLookup
    {
        public int HotelId { get; set; }
        public string RoomNumber { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
