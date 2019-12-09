using System;
using System.Xml.Serialization;

namespace OutboundAdapter.Interfaces.Opera.Models
{
    //[XmlRoot("Envelope", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
    //public class EnvelopeDto<T>
    //{
    //    [XmlElement("Header")]
    //    public HeaderDto Header;

    //    [XmlElement("Body")]
    //    public BodyDto<T> Body;
    //}

    public class HeaderDto
    {
        [XmlElement("Action", Namespace = "http://schemas.xmlsoap.org/ws/2004/08/addressing")]
        public string Action;

        [XmlElement("From", Namespace = "http://schemas.xmlsoap.org/ws/2004/08/addressing")]
        public AddressDto From;

        [XmlElement("MessageID", Namespace = "http://schemas.xmlsoap.org/ws/2004/08/addressing")]
        public string MessageId;

        [XmlElement("ReplyTo", Namespace = "http://schemas.xmlsoap.org/ws/2004/08/addressing")]
        public AddressDto ReplyTo;

        [XmlElement("To", Namespace = "http://schemas.xmlsoap.org/ws/2004/08/addressing")]
        public string To;

        [XmlElement("Security", Namespace = "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd")]
        public SecurityDto Security;
    }

    public class AddressDto
    {
        [XmlElement("Address", Namespace = "http://schemas.xmlsoap.org/ws/2004/08/addressing")]
        public string Address;
    }

    public class SecurityDto
    {
        [XmlAttribute("mustUnderstand", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
        public int MustUnderstand;

        [XmlElement("Timestamp", Namespace = "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd")]
        public TimeStampDto Timestamp;

        [XmlElement("UsernameToken", Namespace = "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd")]
        public UsernameTokenDto UsernameToken;
    }

    public class TimeStampDto
    {
        // I don't think we need this
        //[XmlAttribute("Id", Namespace = "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd")]
        //public string Id;

        // TOOD: Z notation UTC
        [XmlElement("Created", Namespace = "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd")]
        public DateTime Created;

        [XmlElement("Expires", Namespace = "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd")]
        public DateTime Expires;
    }

    public class UsernameTokenDto
    {
        [XmlElement("Username", Namespace = "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd")]
        public string Username;

        [XmlElement("Password", Namespace = "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd")]
        public PasswordDto Password;
    }

    public class PasswordDto
    {
        [XmlAttribute("Type")]
        public string Type;

        [XmlText]
        public string Password;
    }

}
