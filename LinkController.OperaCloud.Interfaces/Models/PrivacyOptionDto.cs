using System.Xml.Serialization;

namespace LinkController.OperaCloud.Interfaces.Models
{
    public class PrivacyOptionDto
    {
        [XmlAttribute("OptionType")]
        public string OptionType { get; set; }

        [XmlAttribute("OptionValue")]
        public string OptionValue { get; set; }
    }
}
