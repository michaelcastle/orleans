using System.Xml.Serialization;

namespace LinkController.OperaCloud.Interfaces.Models
{
    public enum AgeQualifyingCode
    {
        [XmlEnum("ADULT")]
        Adult,

        [XmlEnum("CHILD")]
        Child,

        [XmlEnum("OTHER")]
        Other,

        [XmlEnum("CHILDBUCKET1")]
        Childbucket1,

        [XmlEnum("CHILDBUCKET2")]
        Childbucket2,

        [XmlEnum("CHILDBUCKET3")]
        Childbucket3
    }
}
