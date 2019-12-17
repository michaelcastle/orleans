using System.Xml.Serialization;

namespace LinkController.OperaCloud.Interfaces.Models
{
    [XmlType(Namespace = "http://htng.org/PWS/2008B/SingleGuestItinerary/Common/Types")]
    public class NamePhoneDto
    {
        [XmlAttribute("phoneType")]
        public string PhoneType { get; set; }

        [XmlAttribute("phoneRole")]
        public string PhoneRole { get; set; }

        [XmlAttribute("primary")]
        public bool IsPrimary { get; set; }

        [XmlElement("PhoneNumber")]
        public string PhoneNumber { get; set; }
    }
}
