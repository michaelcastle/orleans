using System;
using System.Collections.Generic;
using System.Text;

namespace OutboundAdapter.Interfaces.Models
{
    public class OrderItem
    {
        public int HotelId { get; set; }
        public int Number { get; set; }
        public Guid PrimaryKey { get; set; }
    }
}
