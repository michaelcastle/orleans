using System.Xml.Serialization;

namespace OutboundAdapter.Interfaces.Opera.Models
{
    public class ResultDto
    {
        [XmlAttribute("resultStatusFlag")]
        public ResultStatusFlag ResultStatusFlag { get; set; }

        [XmlAttribute("code")]
        public int Code { get; set; }
    }
}
