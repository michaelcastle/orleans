using System;
using System.Collections.Generic;
using System.Text;

namespace OutboundAdapter.Interfaces.Models
{
    public class RoomStatusUpdate
    {
        public string ResortId { get; set; }
        public string RoomNumber { get; set; }
        public string RoomStatus { get; set; }
        public string RoomType { get; set; }
    }
}
