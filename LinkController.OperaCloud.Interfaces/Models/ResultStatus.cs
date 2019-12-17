using System.Xml.Serialization;

namespace LinkController.OperaCloud.Interfaces.Models
{
    public enum ResultStatusFlag
    {
        [XmlEnum("SUCCESS")]
        Success,

        [XmlEnum("FAIL")]
        Fail
    }
}
