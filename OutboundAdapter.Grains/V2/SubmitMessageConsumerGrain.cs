using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Streams;
using OutboundAdapter.Interfaces;
using ServiceExtensions.PmsAdapter.PmsProcessor;
using System;
using System.Threading.Tasks;

namespace OutboundAdapter.Grains.V2
{
    public interface ISubmitMessageConsumerGrain : IGrainWithGuidKey
    {
        Task BecomeConsumer(Guid streamId, string streamNamespace, string providerToUse);
        Task StopConsuming();
        Task SubmitMessage(string message);
    }

    public class SubmitMessageConsumerGrain : Grain, ISubmitMessageConsumerGrain
    {
        private IAsyncObservable<string> consumer;
        internal ILogger Logger;
        private IAsyncObserver<string> consumerObserver;
        private StreamSubscriptionHandle<string> consumerHandle;
        private readonly IPmsProcessorService _pmsProcessorService;
        private IHotelPmsGrain _hotel;

        private int HotelId => (int)this.GetPrimaryKeyLong();

        public SubmitMessageConsumerGrain(ILogger<SubmitMessageConsumerGrain> logger, IPmsProcessorService pmsProcessorService)
        {
            _pmsProcessorService = pmsProcessorService;
            Logger = logger;
        }

        public override Task OnActivateAsync()
        {
            _hotel = GrainFactory.GetGrain<IHotelPmsGrain>(HotelId);
            Logger.LogInformation("OnActivateAsync");
            consumerHandle = null;
            return Task.CompletedTask;
        }

        public async Task BecomeConsumer(Guid streamId, string streamNamespace, string providerToUse)
        {
            Logger.LogInformation("BecomeConsumer");
            consumerObserver = new SubmitMessageConsumer(this);
            IStreamProvider streamProvider = base.GetStreamProvider(providerToUse);
            consumer = streamProvider.GetStream<string>(streamId, streamNamespace);
            consumerHandle = await consumer.SubscribeAsync(consumerObserver);
        }

        public async Task StopConsuming()
        {
            Logger.LogInformation("StopConsuming");
            if (consumerHandle != null)
            {
                await consumerHandle.UnsubscribeAsync();
                consumerHandle = null;
            }
        }

        public override Task OnDeactivateAsync()
        {
            Logger.LogInformation("OnDeactivateAsync");
            return Task.CompletedTask;
        }

        public async Task SubmitMessage(string message)
        {
            var config = await _hotel.GetInboundConfiguration();

            var success = await _pmsProcessorService.SubmitMessage(config.Credentials.EncryptedUsername, config.Credentials.EncryptedPassword, message, HotelId.ToString());
        }
    }
}
