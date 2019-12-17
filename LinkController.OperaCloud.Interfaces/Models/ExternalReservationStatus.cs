using System.Xml.Serialization;

namespace LinkController.OperaCloud.Interfaces.Models
{
    public enum ExternalReservationStatus
    {
        [XmlEnum("CHECKED_IN")]
        CHECKED_IN,

        [XmlEnum("CANCELLED")]
        CANCELLED,

        [XmlEnum("CHECKED_OUT")]
        CHECKED_OUT,

        [XmlEnum("RESERVED")]
        RESERVED,

        [XmlEnum("WAITLISTED")]
        WAITLISTED,

        [XmlEnum("OTHER")]
        OTHER,

        [XmlEnum("REVERSE_CHECKED_IN")]
        REVERSE_CHECKED_IN,

        [XmlEnum("REVERSE_CHECKED_OUT")]
        REVERSE_CHECKED_OUT,

        [XmlEnum("NO_SHOW")]
        NO_SHOW
    }
}
