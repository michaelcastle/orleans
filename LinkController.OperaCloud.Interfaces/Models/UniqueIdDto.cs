using System.Xml.Serialization;

namespace LinkController.OperaCloud.Interfaces.Models
{
    [XmlRoot("ProfileIDs", ElementName = "UniqueID")]
    public class UniqueIdDto
    {
        [XmlAttribute("source")]
        public string Source { get; set; }

        [XmlText]
        public string Value { get; set; }
    }
}
