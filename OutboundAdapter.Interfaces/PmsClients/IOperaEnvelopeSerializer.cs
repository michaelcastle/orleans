using OutboundAdapter.Interfaces.Models.Opera;
using System.Xml;

namespace OutboundAdapter.Interfaces.PmsClients
{
    public interface IOperaEnvelopeSerializer
    {
        string Serialize<T>(T envelope, XmlQualifiedName[] xmlNamespaces);
        T Deserialize<T>(string xmlString);
        HeaderDto GetHeaderRequest(string action);
    }
}
