using System.Collections.Generic;
using System.Xml.Serialization;

namespace LinkController.OperaCloud.Interfaces.Models
{
    public class GuestPreferenceDto
    {
        [XmlElement("Type")]
        public string Type { get; set; }

        [XmlElement("Value")]
        public string Value { get; set; }

        [XmlElement("Description")]
        public DescriptionDto Description { get; set; }
    }

    public class DescriptionDto
    {
        [XmlArray("Text")]
        [XmlArrayItem("TextElement")]
        public List<string> Text { get; set; }
    }
}
