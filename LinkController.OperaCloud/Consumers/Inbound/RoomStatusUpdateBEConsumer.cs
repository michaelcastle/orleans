using LinkController.OperaCloud.Interfaces;
using LinkController.OperaCloud.Interfaces.Outbound.Inbound;
using Orleans;
using Orleans.Streams;
using OutboundAdapter.Interfaces;
using OutboundAdapter.Interfaces.Consumer;
using OutboundAdapter.Interfaces.Models;
using OutboundAdapter.Interfaces.StreamHelpers;
using System;
using System.Threading.Tasks;

namespace LinkController.OperaCloud.Consumers.Inbound
{
    public class RoomStatusUpdateBEConsumer : Grain, ISubmitMessageApiConsumer
    {
        private readonly IClusterClient _clusterClient;
        private InboundConfiguration _configuration;
        private IAsyncObservable<string> consumer;
        private StreamSubscriptionHandle<string> consumerHandle;
        private readonly IStreamProvider _streamProvider;
        private readonly IStreamNamespaces _streamNamspaces;

        public RoomStatusUpdateBEConsumer(IClusterClient clusterClient, IStreamProvider streamProvider, IStreamNamespaces streamNamspaces)
        {
            _clusterClient = clusterClient;
            _streamProvider = streamProvider;
            _streamNamspaces = streamNamspaces;
        }

        public override async Task OnActivateAsync()
        {
            await base.OnActivateAsync();
        }

        public async Task SetConfiguration(InboundConfiguration configuration)
        {
            _configuration = configuration;
            //await _configuration.WriteStateAsync();
        }

        public async Task OnNextAsync(string content, StreamSequenceToken token)
        {
            if (_configuration == null || string.IsNullOrEmpty(_configuration.Url))
            {
                throw new Exception("Hotel is not connected to the PMS. Please Connect first.");
            }

            var response = GetResponseContent(content);

            var streamNamespace = _streamNamspaces.InboundNamespace<RoomStatusUpdate, Subscribers.Htng>();
            var stream = _streamProvider.GetStream<RoomStatusUpdate>(this.GetPrimaryKey(), streamNamespace);

            await stream.OnNextAsync(await response);
        }

        private async Task<RoomStatusUpdate> GetResponseContent(string content)
        {
            var mapper = _clusterClient.GetGrain<IInboundMappingOperaCloudGrains>(0);
            var request = await mapper.MapRoomStatusUpdateBE(content);
            var response = new RoomStatusUpdate
            {
                ResortId = request.ResortId,
                RoomNumber = request.RoomNumber,
                RoomStatus = request.RoomStatus,
                RoomType = request.RoomType
            };
            return response;
        }

        Task IAsyncObserver<string>.OnCompletedAsync()
        {
            throw new NotImplementedException();
        }

        public Task OnErrorAsync(Exception ex)
        {
            throw new NotImplementedException();
        }

        public async Task BecomeConsumer(Guid streamId, string streamNamespace, string providerToUse)
        {
            var streamProvider = GetStreamProvider(providerToUse);
            consumer = streamProvider.GetStream<string>(streamId, streamNamespace);
            consumerHandle = await consumer.SubscribeAsync(OnNextAsync, OnErrorAsync, OnActivateAsync);
        }

        public async Task StopConsuming()
        {
            if (consumerHandle != null)
            {
                await consumerHandle.UnsubscribeAsync();
                consumerHandle = null;
            }
        }
    }
}
