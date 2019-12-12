using System.Xml.Serialization;

namespace OutboundAdapter.Interfaces.Opera.Models
{
    public enum ResultStatusFlag
    {
        [XmlEnum("SUCCESS")]
        Success,

        [XmlEnum("FAIL")]
        Fail
    }
}
