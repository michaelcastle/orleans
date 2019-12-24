using Orleans;
using OutboundAdapter.Interfaces.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OutboundAdapter.Interfaces
{
    public interface IHotelPmsGrain : ILinkControllerGrain
    {
        Task Subscribe<T>(string provider, ISubscribeEndpoint configuration) where T : IGrain, ISubscribeWithNamespaceObserver;
        Task UnsubscribeHtng<T>(ISubscribeEndpoint configuration) where T : IGrain, ISubscribeWithNamespaceObserver;
    }

    public interface ILinkControllerGrain : IGrainWithIntegerKey
    {
        Task<bool> IsOutboundConnected();
        Task<PmsConfiguration> GetOutboundConfiguration();
        Task SubscribeResponses(string provider, IList<string> streamNamespaces, ISubscribeObserver observer);
    }

}
