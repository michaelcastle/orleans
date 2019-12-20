using System;

namespace OutboundAdapter.Interfaces.Models
{
    public class OrderItem
    {
        public int HotelId { get; set; }
        public int Number { get; set; }
        public Guid PrimaryKey { get; set; }
        public string Response { get; set; }
        public int TotalNumber { get; set; }
    }
}
