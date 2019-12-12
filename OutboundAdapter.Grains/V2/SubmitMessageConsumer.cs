using Microsoft.Extensions.Logging;
using Orleans.Streams;
using System;
using System.Threading.Tasks;

namespace OutboundAdapter.Grains.V2
{
    public class SubmitMessageConsumer : IAsyncObserver<string>
    {
        private readonly SubmitMessageConsumerGrain hostingGrain;

        internal SubmitMessageConsumer(SubmitMessageConsumerGrain hostingGrain)
        {
            this.hostingGrain = hostingGrain;
        }

        public async Task OnNextAsync(string item, StreamSequenceToken token = null)
        {
            hostingGrain.Logger.LogInformation("OnNextAsync(item={0}, token={1})", item, token != null ? token.ToString() : "null");

            await hostingGrain.SubmitMessage(item);
        }

        public Task OnCompletedAsync()
        {
            hostingGrain.Logger.LogInformation("OnCompletedAsync()");
            return Task.CompletedTask;
        }

        public Task OnErrorAsync(Exception ex)
        {
            hostingGrain.Logger.LogInformation("OnErrorAsync({0})", ex);
            return Task.CompletedTask;
        }
    }
}
