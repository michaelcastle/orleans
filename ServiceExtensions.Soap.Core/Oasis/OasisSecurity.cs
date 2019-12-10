using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace ServiceExtensions.Soap.Core.Oasis
{
    [DataContract(Name = OasisConstants.SecurityRootNode, Namespace = OasisConstants.SecurityNamespace)]
    [XmlRoot(OasisConstants.SecurityRootNode, Namespace = OasisConstants.SecurityNamespace)]
    public class OasisSecurity
    {
        private readonly XmlSerializerNamespaces _xmlns = new XmlSerializerNamespaces();

        public OasisSecurity()
        {
            _xmlns.Add(OasisConstants.SecurityNamespaceAlias, OasisConstants.SecurityNamespace);
            _xmlns.Add(OasisConstants.UtilityNamespaceAlias, OasisConstants.UtilityNamespace);
        }

        [DataMember(Name = OasisConstants.TimestampRootNode, Order = 1, IsRequired = false)]
        [XmlElement(OasisConstants.TimestampRootNode, Namespace = OasisConstants.UtilityNamespace)]
        public OasisTimestamp Timestamp { get; set; }

        [DataMember(Name = OasisConstants.UsernameTokenRootNode, Order = 2, IsRequired = true)]
        [XmlElement]
        public OasisUsernameToken UsernameToken { get; set; }
    }
}
