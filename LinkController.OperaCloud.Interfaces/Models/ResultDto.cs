using System.Xml.Serialization;

namespace LinkController.OperaCloud.Interfaces.Models
{
    public class ResultDto
    {
        [XmlAttribute("resultStatusFlag")]
        public ResultStatusFlag ResultStatusFlag { get; set; }

        [XmlAttribute("code")]
        public int Code { get; set; }
    }
}
