using System.Xml.Serialization;

namespace OutboundAdapter.Interfaces.Opera.Models
{
    public class DataElementDto
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("newData")]
        public string NewData { get; set; }

        [XmlAttribute("oldData")]
        public string OldData { get; set; }
    }
}
