using System.Xml.Serialization;

namespace LinkController.OperaCloud.Interfaces.Models
{
    [XmlType(Namespace = "http://htng.org/PWS/2008B/SingleGuestItinerary/Common/Types")]
    public class PersonNameDto
    {
        [XmlAttribute("familiarName")]
        public string FamiliarName { get; set; }

        [XmlElement("NameTitle")]
        public string NameTitle { get; set; }

        [XmlElement("FirstName")]
        public string FirstName { get; set; }

        [XmlElement("MiddleName")]
        public string MiddleName { get; set; }

        [XmlElement("LastName")]
        public string LastName { get; set; }

        [XmlElement("NameSuffix")]
        public string NameSuffix { get; set; }
    }
}
