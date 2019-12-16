using Orleans;
using Orleans.Streams;
using OutboundAdapter.Interfaces.Models;
using System;
using System.Threading.Tasks;

namespace OutboundAdapter.Interfaces
{
    public interface ISubscribeToResponseObserver : IAsyncObserver<string>, IGrainWithIntegerKey
    {
        Task SetConfiguration(InboundConfiguration configuration);
        Task BecomeConsumer(Guid streamId, string streamNamespace, string providerToUse);
    }
}
