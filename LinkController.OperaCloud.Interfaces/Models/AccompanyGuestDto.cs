using System.Xml.Serialization;

namespace LinkController.OperaCloud.Interfaces.Models
{
    public class AccompanyGuestDto
    {
        [XmlElement("NameID")]
        public int NameID { get; set; }

        [XmlElement("FirstName")]
        public string FirstName { get; set; }

        [XmlElement("LastName")]
        public string LastName { get; set; }
    }
}
