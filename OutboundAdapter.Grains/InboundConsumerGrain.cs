using Orleans;
using Orleans.Concurrency;
using OutboundAdapter.Interfaces;
using OutboundAdapter.Interfaces.Consumer;
using OutboundAdapter.Interfaces.Models;
using System.Threading.Tasks;

namespace OutboundAdapter.Grains
{
    [StatelessWorker]
    public class InboundConsumerGrain : Grain, IInboundConsumerGrain
    {
        // TODO: Add V2 Generic and Topcat observers, create service to get these
        Task<ISubscribeToResponseObserver> IInboundConsumerGrain.GetInboundConsumer(InboundConfiguration configuration, int hotelId)
        {
            ISubscribeToResponseObserver observer;
            observer = configuration.InboundType switch
            {
                "V2" => GrainFactory.GetGrain<ISubmitMessageConsumer>(hotelId),
                "Htng" => GrainFactory.GetGrain<ISubmitMessageApiConsumer>(hotelId),
                "Topcat" => GrainFactory.GetGrain<ISubmitMessageApiConsumer>(hotelId),
                _ => null,
            };

            return Task.FromResult(observer);
        }
    }
}
