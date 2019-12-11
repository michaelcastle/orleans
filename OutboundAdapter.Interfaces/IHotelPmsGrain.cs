using OutboundAdapter.Interfaces.Models;
using System.Threading.Tasks;

namespace OutboundAdapter.Interfaces
{
    public interface IHotelPmsGrain : Orleans.IGrainWithIntegerKey
    {
        Task<string> StreamNamespaceOutbound<T>();
        Task<string> StreamNamespaceInbound<T>();
        Task<bool> IsOutboundConnected();
        Task<bool> IsInboundConnected();
        Task<InboundConfiguration> GetInboundConfiguration();
        Task<OutboundConfiguration> GetOutboundConfiguration();
        Task SubscribeTo(InboundConfiguration configuration);
        Task IncrementAsync();
        Task SaveOutboundConfigurationAsync(OutboundConfiguration configuration);
    }
}
