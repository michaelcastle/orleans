using System.Xml.Serialization;

namespace LinkController.OperaCloud.Interfaces.Models
{
    [XmlRoot("UpdateProfileRequest", Namespace = "http://htng.org/PWS/2008B/SingleGuestItinerary/Name/Types")]
    public class UpdateProfileRequestDto
    {
        [XmlElement("ResortId")]
        public string ResortId { get; set; }

        [XmlElement("Profile")]
        public ProfileDto Profile { get; set; }
    }
}
