using System;
using System.Xml.Serialization;

namespace LinkController.OperaCloud.Interfaces.Models
{
    public class UserDefinedValueDto
    {
        [XmlAttribute("valueName")]
        public string ValueName { get; set; }

        [XmlElement("DateValue")]
        public DateTime DateValue { get; set; }

        [XmlElement("CharacterValue")]
        public string CharacterValue { get; set; }

        [XmlElement("NumericValue")]
        public double NumericValue { get; set; }
    }
}
