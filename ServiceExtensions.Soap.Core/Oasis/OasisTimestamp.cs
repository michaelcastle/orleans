using System;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Serialization;

namespace ServiceExtensions.Soap.Core.Oasis
{
    [DataContract(Name = OasisConstants.TimestampRootNode)]
    public class OasisTimestamp
    {
        [XmlAttribute(OasisConstants.TimestampIdAttribute, Namespace = OasisConstants.UtilityNamespace)]
        public string Id { get; set; }

        [XmlElement(OasisConstants.TimestampCreatedNode)]
        public string CreatedString
        {
            get => XmlConvert.ToString(Created);
            set => Created = DateTimeOffset.Parse(value);
        }

        [XmlIgnore]
        public DateTimeOffset Created { get; set; }

        [XmlElement(OasisConstants.TimestampExpiresNode)]
        public string ExpiresString
        {
            get => XmlConvert.ToString(Expires);
            set => Expires = DateTimeOffset.Parse(value);
        }

        [XmlIgnore]
        public DateTimeOffset Expires { get; set; }
    }
}
