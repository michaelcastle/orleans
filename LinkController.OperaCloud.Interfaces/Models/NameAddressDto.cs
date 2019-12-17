using System.Xml.Serialization;

namespace LinkController.OperaCloud.Interfaces.Models
{
    [XmlType(Namespace = "http://htng.org/PWS/2008B/SingleGuestItinerary/Common/Types")]
    public class NameAddressDto
    {
        [XmlAttribute("addressType")]
        public string AddressType { get; set; }

        [XmlAttribute("primary")]
        public bool IsPrimary { get; set; }

        [XmlElement("AddressLine")]
        public string AddressLine { get; set; }

        [XmlElement("CityName")]
        public string CityName { get; set; }

        [XmlElement("StateProv")]
        public string StateProv { get; set; }

        [XmlElement("CountryCode")]
        public string CountryCode { get; set; }

        [XmlElement("PostalCode")]
        public string PostalCode { get; set; }
    }
}
