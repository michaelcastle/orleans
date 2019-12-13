using OutboundAdapter.Interfaces.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OutboundAdapter.Interfaces
{
    public interface IHotelPmsGrain : Orleans.IGrainWithIntegerKey
    {
        Task<string> StreamNamespaceOutbound<T>();
        Task<string> StreamNamespaceInbound<T, P>();
        Task<bool> IsOutboundConnected();
        Task<bool> IsInboundConnected();
        Task<InboundConfiguration> GetInboundConfiguration();
        Task<OutboundConfiguration> GetOutboundConfiguration();
        Task SubscribeToResponses(InboundConfiguration configuration, string provider, IList<string> streamNamespaces, ISubscribeToResponseObserver observer);
        Task IncrementAsync();
        Task SaveOutboundConfigurationAsync(OutboundConfiguration configuration);
    }
}
