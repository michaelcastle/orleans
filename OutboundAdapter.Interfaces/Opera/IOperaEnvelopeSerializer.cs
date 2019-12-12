using OutboundAdapter.Interfaces.Opera.Models;
using System.Xml;

namespace OutboundAdapter.Interfaces.Opera
{
    public interface IOperaEnvelopeSerializer
    {
        string Serialize<T>(T envelope, XmlQualifiedName[] xmlNamespaces);
        T Deserialize<T>(string xmlString);
        T DeserialiseNode<T>(string message, string nodeName);
        HeaderDto GetHeaderRequest(string action);
    }
}
