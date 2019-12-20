using ServiceExtensions.Soap.Core.Oasis;
using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Xml;

namespace ServiceExtensions.Soap.Core.Response
{
    public class OasisResponseMessage : Message
    {
        private readonly Message _message;
        private readonly MessageHeaders _messageHeaders;

        public OasisResponseMessage(MessageHeaders messageHeaders, Message message)
        {
            _message = message;
            _messageHeaders = messageHeaders;
        }

        public override MessageHeaders Headers => _message.Headers;

        public override MessageProperties Properties => _message.Properties;

        public override MessageVersion Version => _message.Version;

        protected override void OnWriteStartEnvelope(XmlDictionaryWriter writer)
        {
            writer.WriteStartDocument();
            if (_message.Version.Envelope == EnvelopeVersion.Soap11)
            {
                writer.WriteStartElement("s", "Envelope", "http://schemas.xmlsoap.org/soap/envelope/");
            }
            else
            {
                writer.WriteStartElement("s", "Envelope", "http://www.w3.org/2003/05/soap-envelope");
            }

            AddSoap11WSAddressingAugust2004(writer);
            AddOasisSecurityAugust2004(writer);

            writer.WriteAttributeString("xmlns:xsd", "http://www.w3.org/2001/XMLSchema");
            writer.WriteAttributeString("xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance");
        }

        protected override void OnWriteBodyContents(XmlDictionaryWriter writer)
        {
            _message.WriteBodyContents(writer);
        }

        private void AddSoap11WSAddressingAugust2004(XmlDictionaryWriter writer)
        {
            var addressingPrefix = "wsa";
            var addressingSchema = "http://schemas.xmlsoap.org/ws/2004/08/addressing";
            var addressingRoleAnnonymous = "http://schemas.xmlsoap.org/ws/2004/08/addressing/role/anonymous";

            // Schema
            writer.WriteAttributeString($"xmlns:{addressingPrefix}", $"{addressingSchema}.xsd");

            // Header
            _message.Headers.Add(MessageHeader.CreateHeader($"{addressingPrefix}:Action", string.Empty, $"{_messageHeaders.Action}Response"));
            _message.Headers.Add(MessageHeader.CreateHeader($"{addressingPrefix}:MessageID", string.Empty, $"urn:uuid:{Guid.NewGuid()}"));
            _message.Headers.Add(MessageHeader.CreateHeader($"{addressingPrefix}:RelatesTo", string.Empty, $"{_messageHeaders.GetHeader<string>("MessageID", addressingSchema)}"));
            _message.Headers.Add(MessageHeader.CreateHeader($"{addressingPrefix}:To", string.Empty, addressingRoleAnnonymous));
        }

        private void AddOasisSecurityAugust2004(XmlDictionaryWriter writer)
        {
            // Schema
            writer.WriteAttributeString($"xmlns:{OasisConstants.SecurityNamespaceAlias}", OasisConstants.SecurityNamespace);
            writer.WriteAttributeString($"xmlns:{OasisConstants.UtilityNamespaceAlias}", OasisConstants.UtilityNamespace);

            // Header
            var currentTime = DateTimeOffset.Now;
            var securityHeader = new OasisTimestampHeader(currentTime, currentTime.AddMinutes(5));
            _message.Headers.Add(securityHeader);
        }
    }
}
