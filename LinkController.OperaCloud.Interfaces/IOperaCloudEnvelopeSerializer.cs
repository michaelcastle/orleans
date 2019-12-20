using LinkController.OperaCloud.Interfaces.Models;
using System.Xml;

namespace LinkController.OperaCloud.Interfaces
{
    public interface IOperaCloudEnvelopeSerializer
    {
        string Serialize<T>(T envelope, XmlQualifiedName[] xmlNamespaces);
        T Deserialize<T>(string xmlString);
        T DeserialiseNode<T>(string message, string nodeName);
        HeaderDto GetHeaderRequest(string action);
    }
}
