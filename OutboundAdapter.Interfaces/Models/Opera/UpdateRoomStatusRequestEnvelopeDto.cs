using System.Xml.Serialization;

namespace OutboundAdapter.Interfaces.Models.Opera
{
    [XmlRoot("Envelope", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
    public class UpdateRoomStatusRequestEnvelopeDto
    {
        [XmlElement("Header")]
        public HeaderDto Header;

        [XmlElement("Body")]
        public UpdateRoomStatusRequestBodyDto Body;
    }

    public class UpdateRoomStatusRequestBodyDto
    {
        [XmlElement("UpdateRoomStatusRequest", Namespace = "http://webservices.micros.com/htng/2008B/SingleGuestItinerary/Housekeeping/Types")]
        public UpdateRoomStatusRequestDto Request;
    }
}
