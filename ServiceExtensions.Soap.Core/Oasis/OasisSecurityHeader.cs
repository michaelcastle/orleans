using System;
using System.ServiceModel.Channels;
using System.Xml;

namespace ServiceExtensions.Soap.Core.Oasis
{
    public class OasisSecurityHeader : MessageHeader
    {
        private readonly string _password;
        private readonly string _username;
        private readonly string _nonce;
        private readonly DateTime _createdDate;

        public OasisSecurityHeader(string id, string username, string password, string nonce)
        {
            _password = password;
            _username = username;
            _nonce = nonce;
            _createdDate = DateTime.Now;
            Id = id;
        }

        public string Id { get; set; }

        public override string Name => "Security";

        public override string Namespace => "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd";

        protected override void OnWriteStartHeader(XmlDictionaryWriter writer, MessageVersion messageVersion)
        {
            writer.WriteStartElement("wsse", Name, Namespace);
            writer.WriteXmlnsAttribute("wsse", Namespace);
        }

        protected override void OnWriteHeaderContents(XmlDictionaryWriter writer, MessageVersion messageVersion)
        {
            writer.WriteStartElement("wsse", "UsernameToken", Namespace);
            writer.WriteAttributeString("Id", "UsernameToken-10");
            writer.WriteAttributeString("wsu", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd");

            writer.WriteStartElement("wsse", "Username", Namespace);
            writer.WriteValue(_username);
            writer.WriteEndElement();

            writer.WriteStartElement("wsse", "Password", Namespace);
            writer.WriteAttributeString("Type", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-username-token-profile-1.0#PasswordText");
            writer.WriteValue(_password);
            writer.WriteEndElement();

            writer.WriteStartElement("wsse", "Nonce", Namespace);
            writer.WriteAttributeString("EncodingType", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-soap-message-security-1.0#Base64Binary");
            writer.WriteValue(_nonce);
            writer.WriteEndElement();

            writer.WriteStartElement("wsse", "Created", Namespace);
            writer.WriteValue(_createdDate.ToString("YYYY-MM-DDThh:mm:ss"));
            writer.WriteEndElement();

            writer.WriteEndElement();
        }
    }
}
