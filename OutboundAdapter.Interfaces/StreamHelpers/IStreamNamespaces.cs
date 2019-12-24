using System.Collections.Generic;

namespace OutboundAdapter.Interfaces.StreamHelpers
{
    public interface IStreamNamespaces
    {
        string InboundNamespace<T, P>();
        string OutboundNamespace<T, P>();
    }

    public interface IStreamV2Namespaces : IStreamNamespaces
    {
        List<string> InboundNamespaces { get; }
    }

    public interface IStreamHtngNamespaces : IStreamNamespaces
    {
        List<string> InboundNamespaces { get; }
    }
}
