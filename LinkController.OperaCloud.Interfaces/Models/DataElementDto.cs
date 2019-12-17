using System.Xml.Serialization;

namespace LinkController.OperaCloud.Interfaces.Models
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
