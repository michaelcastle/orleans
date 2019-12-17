using System.Xml.Serialization;

namespace LinkController.OperaCloud.Interfaces.Models
{
    public class CustomerDto
    {
        [XmlElement("PersonName", Namespace = "http://htng.org/PWS/2008B/SingleGuestItinerary/Name/Types")]
        public PersonNameDto Person { get; set; }
    }
}
