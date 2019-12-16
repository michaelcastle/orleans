using Orleans;
using Orleans.Streams;
using OutboundAdapter.Interfaces;
using OutboundAdapter.Interfaces.Consumer;
using OutboundAdapter.Interfaces.Models;
using System;
using System.Threading.Tasks;

namespace OutboundAdapter.Grains.Api
{
    public class SubmitMessageApiConsumer : Grain, ISubmitMessageApiConsumer
    {
        public Task BecomeConsumer(Guid streamId, string streamNamespace, string providerToUse)
        {
            throw new NotImplementedException();
        }

        Task IAsyncObserver<string>.OnCompletedAsync()
        {
            throw new NotImplementedException();
        }

        Task IAsyncObserver<string>.OnErrorAsync(Exception ex)
        {
            throw new NotImplementedException();
        }

        Task IAsyncObserver<string>.OnNextAsync(string item, StreamSequenceToken token)
        {
            throw new NotImplementedException();
        }

        Task ISubscribeToResponseObserver.SetConfiguration(InboundConfiguration configuration)
        {
            throw new NotImplementedException();
        }
    }
}
