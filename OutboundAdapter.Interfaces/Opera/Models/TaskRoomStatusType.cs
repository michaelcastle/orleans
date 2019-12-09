using System.Xml.Serialization;

namespace OutboundAdapter.Interfaces.Opera.Models
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
