using System.Collections.Generic;
using System.Xml.Serialization;

namespace LinkController.OperaCloud.Interfaces.Models
{
    public class ProfileDto
    {
        [XmlAttribute("nameType")]
        public string NameType { get; set; }

        [XmlAttribute("vipCode")]
        public string VipCode { get; set; }

        [XmlArray("IDs")]
        [XmlArrayItem("UniqueID", Namespace = "http://htng.org/PWS/2008B/SingleGuestItinerary/Common/Types")]
        public List<UniqueIdDto> ProfileIds { get; set; }

        [XmlElement("Customer", Namespace = "http://htng.org/PWS/2008B/SingleGuestItinerary/Name/Types")]
        public CustomerDto Customer { get; set; }

        [XmlArray("Addresses")]
        [XmlArrayItem("NameAddress", Namespace = "http://htng.org/PWS/2008B/SingleGuestItinerary/Name/Types")]
        public List<NameAddressDto> Addresses { get; set; }

        [XmlArray("Phones")]
        [XmlArrayItem("NamePhone", Namespace = "http://htng.org/PWS/2008B/SingleGuestItinerary/Name/Types")]
        public List<NamePhoneDto> Phones { get; set; }

        [XmlArray("PrivacyList")]
        [XmlArrayItem("PrivacyOption", Namespace = "http://htng.org/PWS/2008B/SingleGuestItinerary/Name/Types")]
        public List<PrivacyOptionDto> PrivacyList { get; set; }

    }
}
