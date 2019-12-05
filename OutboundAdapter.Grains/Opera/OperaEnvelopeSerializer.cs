﻿using NodaTime;
using OutboundAdapter.Interfaces.Models.Opera;
using OutboundAdapter.Interfaces.PmsClients;
using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace OutboundAdapter.Grains.Opera
{
    public class OperaEnvelopeSerializer : IOperaEnvelopeSerializer
    {
        private readonly IClock _clock;

        public OperaEnvelopeSerializer(IClock clock)
        {
            _clock = clock;
        }

        public string Serialize<T>(T envelope, XmlQualifiedName[] xmlNamespaces)
        {
            var settings = new XmlWriterSettings
            {
                OmitXmlDeclaration = true,
                Encoding = Encoding.UTF8
            };

            var namespaces = new XmlSerializerNamespaces(xmlNamespaces);
            namespaces.Add("s", "http://schemas.xmlsoap.org/soap/envelope/");
            namespaces.Add("soap", "http://schemas.xmlsoap.org/soap/envelope/");
            namespaces.Add("xsi", "http://www.w3.org/2001/XMLSchema-instance");
            namespaces.Add("xsd", "http://www.w3.org/2001/XMLSchema");
            namespaces.Add("wsa", "http://schemas.xmlsoap.org/ws/2004/08/addressing");
            namespaces.Add("wsse", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd");
            namespaces.Add("wsu", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd");

            var serializer = new XmlSerializer(typeof(T));

            var stringBuilder = new StringBuilder();
            var xmlWriter = XmlWriter.Create(stringBuilder, settings);

            // Serialize object to xml
            serializer.Serialize(xmlWriter, envelope, namespaces);

            xmlWriter.Flush();
            return stringBuilder.ToString();
        }

        public T Deserialize<T>(string xmlString)
        {
            T requestDto;
            var serializer = new XmlSerializer(typeof(T));
            using (var reader = new StringReader(xmlString))
            {
                requestDto = (T)serializer.Deserialize(reader);
            }

            return requestDto;
        }

        public HeaderDto GetHeaderRequest(string action)
        {
            var EndpointReferenceUri = "http://www.micros.com/HTNGActivity/";
            var EndpointReplyToUri = "http://schemas.xmlsoap.org/ws/2004/08/addressing/role/anonymous";
            var Address = "OPTII";
            var HtngUsername = "OPTIIRI3OPUYAYICRLTE";
            var HtngPassword = "7r3gu6RacRetREw7puvABrusWehAgE";

            var header = new HeaderDto
            {
                Action = action,
                From = new AddressDto
                {
                    Address = $"urn:{Address}"
                },
                MessageId = $"urn:uuid:{Guid.NewGuid()}",
                ReplyTo = new AddressDto
                {
                    Address = EndpointReplyToUri
                },
                To = EndpointReferenceUri,
                Security = new SecurityDto
                {
                    MustUnderstand = 1,
                    Timestamp = new TimeStampDto
                    {
                        Created = _clock.GetCurrentInstant().ToDateTimeUtc(),
                        Expires = _clock.GetCurrentInstant().ToDateTimeUtc().AddMinutes(5)
                    },
                    UsernameToken = new UsernameTokenDto
                    {
                        Username = HtngUsername,
                        Password = new PasswordDto
                        {
                            Password = HtngPassword,
                            Type = "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-username-token-profile-1.0#PasswordText"
                        }
                    }
                }
            };

            return header;
        }
    }
}