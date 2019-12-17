using System.Xml.Serialization;

namespace LinkController.OperaCloud.Interfaces.Models
{
    [XmlRoot("FetchRoomStatusRequest", Namespace = "http://webservices.micros.com/htng/2008B/SingleGuestItinerary/Housekeeping/Types")]
    public class FetchRoomStatusRequestDto
    {
        [XmlElement("ResortId")]
        public string ResortId { get; set; }

        [XmlElement("RoomNumber")]
        public string RoomNumber { get; set; }
    }
}
