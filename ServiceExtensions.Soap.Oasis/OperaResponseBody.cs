using System;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace ServiceExtensions.Soap.Oasis
{
    [XmlType]
    public class OperaResponseBody : IXmlSerializable
    {
        public OperaResponseBody(Exception ex, bool returnError = true)
        {
            Result = Flag.FAIL;
            FailReason = ex;
            ReturnError = returnError;
        }

        public OperaResponseBody(Flag result)
        {
            Result = result;
        }

        public bool ReturnError { get; set; }

        public Flag Result { get; set; }

        public Exception FailReason { get; private set; }

        public XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
        }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteAttributeString("resultStatusFlag", Result.ToString());
            if (ReturnError && FailReason != null && !string.IsNullOrEmpty(FailReason.Message))
            {
                writer.WriteStartElement("Text", "http://htng.org/PWS/2008B/SingleGuestItinerary/Common/Types");
                writer.WriteStartElement("TextElement");
                writer.WriteValue(FailReason.Message);
                writer.WriteEndElement();
                writer.WriteEndElement();
            }
        }
    }

    public enum Flag
    {
        SUCCESS,
        FAIL
    }
}
