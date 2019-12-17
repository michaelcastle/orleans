using System.Xml.Serialization;

namespace LinkController.OperaCloud.Interfaces.Models
{
    public enum ActionType
    {
        [XmlEnum("NEW")]
        NEW,

        [XmlEnum("UPDATE")]
        UPDATE,

        [XmlEnum("DELETE")]
        DELETE
    }
}
