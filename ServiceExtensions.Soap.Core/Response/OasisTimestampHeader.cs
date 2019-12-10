using System;
using System.ServiceModel.Channels;
using System.Xml;

namespace ServiceExtensions.Soap.Core.Response
{
    public class OasisTimestampHeader : MessageHeader
    {
        private const string DateTimeZuluTimeFormat = "yyyy-MM-ddTHH:mm:ssZ";

        private string _namespaceSuffix = "wsu";
        private DateTimeOffset _created;
        private DateTimeOffset _expires;

        public OasisTimestampHeader(DateTimeOffset created, DateTimeOffset expires)
        {
            _created = created;
            _expires = expires;
        }

        public override string Name => "Security";

        public override string Namespace => "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd";

        protected override void OnWriteStartHeader(XmlDictionaryWriter writer, MessageVersion messageVersion)
        {
            writer.WriteStartElement("wsse:Security");
        }

        protected override void OnWriteHeaderContents(XmlDictionaryWriter writer, MessageVersion messageVersion)
        {
            writer.WriteStartElement($"{_namespaceSuffix}:Timestamp");
            writer.WriteAttributeString($"{_namespaceSuffix}:Id", $"Timestamp-{Guid.NewGuid()}");

            writer.WriteStartElement($"{_namespaceSuffix}:Created");
            writer.WriteValue(_created.ToUniversalTime().ToString(DateTimeZuluTimeFormat));
            writer.WriteEndElement();

            writer.WriteStartElement($"{_namespaceSuffix}:Expires");
            writer.WriteValue(_expires.ToUniversalTime().ToString(DateTimeZuluTimeFormat));
            writer.WriteEndElement();

            writer.WriteEndElement();
        }
    }
}
