namespace OutboundAdapter.Interfaces.StreamHelpers
{
    public interface IStreamNamespaces
    {
        string InboundNamespace<T, P>();
        string OutboundNamespace<T, P>();
    }
}
