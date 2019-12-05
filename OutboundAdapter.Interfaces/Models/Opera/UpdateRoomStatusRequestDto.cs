using System.Xml.Serialization;

namespace OutboundAdapter.Interfaces.Models.Opera
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
                switch (_roomStatus)
                {
                    case ("CL"):
                        return "Clean";
                    case ("DI"):
                        return "Dirty";
                    case ("IP"):
                        return "Inspected";
                    case ("PU"):
                        return "Pickup";
                    default:
                        return _roomStatus;
                }
            }
            set { _roomStatus = value; }
        }
    }
}
