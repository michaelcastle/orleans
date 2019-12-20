namespace OutboundAdapter.Interfaces.StreamHelpers
{
    public class StreamNamespaces : IStreamNamespaces
    {
        public string InboundNamespace<T, P>()
        {
            return $"Inbound{typeof(T).Name}{typeof(P).Name}";
        }

        public string OutboundNamespace<T, P>()
        {
            return $"Outbound{typeof(T).Name}{typeof(P).Name}";
        }
    }
}
