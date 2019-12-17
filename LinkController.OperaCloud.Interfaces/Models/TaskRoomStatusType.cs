using System.Xml.Serialization;

namespace LinkController.OperaCloud.Interfaces.Models
{
    public enum TaskRoomStatusType
    {
        [XmlEnum("Clean")]
        Clean,

        [XmlEnum("Dirty")]
        Dirty,

        [XmlEnum("Inspected")]
        Inspected,

        [XmlEnum("Pickup")]
        Pickup,

        [XmlEnum("OutOfOrder")]
        OutOfOrder,

        [XmlEnum("OutOfService")]
        OutOfService
    }
}
