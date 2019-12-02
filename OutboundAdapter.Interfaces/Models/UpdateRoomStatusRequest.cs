namespace OutboundAdapter.Interfaces.Models
{
    public class UpdateRoomStatusRequest
    {
        public int HotelId { get; set; }
        public string RoomNumber { get; set; }
        public string RoomStatus { get; set; }
    }
}
