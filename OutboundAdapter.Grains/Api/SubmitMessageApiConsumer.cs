using Orleans;
using Orleans.Streams;
using OutboundAdapter.Interfaces.Consumer;
using OutboundAdapter.Interfaces.Models;
using System;
using System.Threading.Tasks;

namespace OutboundAdapter.Grains.Api
{
    public class SubmitMessageApiConsumer : Grain, ISubmitMessageHtngConsumer
    {
        public Task BecomeConsumer(Guid streamId, string streamNamespace, string providerToUse)
        {
            throw new NotImplementedException();
        }

        public Task<string> Namespace()
        {
            throw new NotImplementedException();
        }

        public Task StopConsuming()
        {
            throw new NotImplementedException();
        }

        public Task OnCompletedAsync()
        {
            throw new NotImplementedException();
        }

        public Task OnErrorAsync(Exception ex)
        {
            throw new NotImplementedException();
        }

        public Task OnNextAsync(string item, StreamSequenceToken token)
        {
            throw new NotImplementedException();
        }

        public Task SetConfiguration(ISubscribeEndpoint configuration)
        {
            throw new NotImplementedException();
        }
    }
}
