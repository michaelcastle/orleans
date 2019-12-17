using System.Xml.Serialization;

namespace LinkController.OperaCloud.Interfaces.Models
{
    public class GuestCountDto
    {
        [XmlAttribute("ageQualifyingCode")]
        public AgeQualifyingCode AgeQualifyingCode { get; set; }

        [XmlAttribute("count")]
        public int Count { get; set; }
    }
}
