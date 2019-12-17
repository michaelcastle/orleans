using System.Xml.Serialization;

namespace LinkController.OperaCloud.Interfaces.Models
{
    public class ProfileInfoDto
    {
        [XmlElement("FirstName", Namespace = "http://htng.org/PWS/2008B/SingleGuestItinerary/Common/Types")]
        public string FirstName { get; set; }

        [XmlElement("LastName", Namespace = "http://htng.org/PWS/2008B/SingleGuestItinerary/Common/Types")]
        public string LastName { get; set; }
    }
}
