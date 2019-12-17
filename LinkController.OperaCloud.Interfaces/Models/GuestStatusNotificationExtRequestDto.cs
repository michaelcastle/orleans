using System.Xml.Serialization;

namespace LinkController.OperaCloud.Interfaces.Models
{
    [XmlRoot("GuestStatusNotificationExtRequest", Namespace = "http://webservices.micros.com/htng/2008B/SingleGuestItinerary/Reservation/Types")]
    public class GuestStatusNotificationExtRequestDto
    {
        [XmlElement("GuestStatus")]
        public GuestStatusDto GuestStatus { get; set; }
    }
}
