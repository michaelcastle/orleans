using System.Xml.Serialization;

namespace LinkController.OperaCloud.Interfaces.Models
{
    public enum Direction
    {
        [XmlEnum("ARRIVAL")]
        Arrival,

        [XmlEnum("DEPARTURE")]
        Departure,

        [XmlEnum("INHOUSE")]
        Inhouse
    }

    public enum TranspoftType
    {
        [XmlEnum("AIR")]
        Air,

        [XmlEnum("TAXI")]
        Taxi,

        [XmlEnum("BUS")]
        Bus,

        [XmlEnum("TRAIN")]
        Train,

        [XmlEnum("BOAT")]
        Boat,

        [XmlEnum("OTHER")]
        Other
    }
}
