using System;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Serialization;

namespace ServiceExtensions.Soap.Core.Oasis
{
    [DataContract(Name = OasisConstants.UsernameTokenRootNode, Namespace = OasisConstants.UtilityNamespace)]
    public class OasisUsernameToken
    {
        [DataMember(Name = OasisConstants.UsernameTokenUsernameNode, Order = 1, IsRequired = true)]
        [XmlElement]
        public string Username { get; set; }

        [DataMember(Name = OasisConstants.UsernameTokenPasswordNode, Order = 2, IsRequired = true)]
        public string Password { get; set; }

        [DataMember(Name = OasisConstants.UsernameTokenNonceNode, Order = 2, IsRequired = true)]
        public string Nonce { get; set; }

        [DataMember(Name = OasisConstants.UsernameTokenCreatedNode, Order = 2, IsRequired = true)]
        [XmlElement(OasisConstants.UsernameTokenCreatedNode, Namespace = OasisConstants.UtilityNamespace)]
        public string CreatedString
        {
            get => XmlConvert.ToString(Created);
            set => Created = DateTimeOffset.Parse(value);
        }

        [XmlIgnore]
        public DateTimeOffset Created { get; set; }
    }
}
