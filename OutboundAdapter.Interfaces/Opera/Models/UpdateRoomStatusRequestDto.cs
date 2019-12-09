using System.Xml.Serialization;

namespace OutboundAdapter.Interfaces.Opera.Models
{
    [XmlRoot("UpdateRoomStatusRequest", Namespace = "http://webservices.micros.com/htng/2008B/SingleGuestItinerary/Housekeeping/Types")]
    public class UpdateRoomStatusRequestDto
    {
        private string _roomStatus;

        [XmlElement("ResortId")]
        public string ResortId { get; set; }

        [XmlElement("RoomNumber")]
        public string RoomNumber { get; set; }

        [XmlElement("RoomStatus")]
        public string RoomStatus
        {
            get
            {
                return _roomStatus switch
                {
                    ("CL") => "Clean",
                    ("DI") => "Dirty",
                    ("IP") => "Inspected",
                    ("PU") => "Pickup",
                    _ => _roomStatus,
                };
            }
            set { _roomStatus = value; }
        }
    }
}
