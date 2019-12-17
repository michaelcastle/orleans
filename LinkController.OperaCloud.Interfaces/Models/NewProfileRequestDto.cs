using System.Xml.Serialization;

namespace LinkController.OperaCloud.Interfaces.Models
{
    [XmlRoot("NewProfileRequest", Namespace = "http://htng.org/PWS/2008B/SingleGuestItinerary/Name/Types")]
    public class NewProfileRequestDto
    {
        [XmlElement("ResortId")]
        public string ResortId { get; set; }

        [XmlElement("Profile")]
        public ProfileDto Profile { get; set; }
    }
}
