using System.Xml.Serialization;

namespace LinkController.OperaCloud.Interfaces.Models
{
    [XmlRoot("Envelope", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
    public class UpdateRoomStatusResponseEnvelopeDto
    {
        [XmlElement("Header")]
        public HeaderDto Header;

        [XmlElement("Body")]
        public UpdateRoomStatusResponseBodyDto Body;
    }

    public class UpdateRoomStatusResponseBodyDto
    {
        [XmlElement("UpdateRoomStatusResponse", Namespace = "http://webservices.micros.com/htng/2008B/SingleGuestItinerary/Housekeeping/Types")]
        public UpdateRoomStatusRequestDto Request;
    }
}
