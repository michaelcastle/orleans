using LinkController.OperaCloud.Interfaces;
using LinkController.OperaCloud.Interfaces.Models;
using LinkController.OperaCloud.Interfaces.Outbound.Inbound;
using Orleans;
using Orleans.Runtime;
using Orleans.Streams;
using OutboundAdapter.Interfaces.Models;
using OutboundAdapter.Interfaces.StreamHelpers;
using System;
using System.Threading.Tasks;

namespace LinkController.OperaCloud.Consumers.Inbound
{
    public class RoomStatusUpdateBEConsumer : Grain, IRoomStatusUpdateBEConsumer
    {
        private readonly IClusterClient _clusterClient;
        private readonly IPersistentState<SubscribeEndpoint> _configuration;
        private IAsyncObservable<string> consumer;
        private StreamSubscriptionHandle<string> consumerHandle;
        private readonly IStreamProvider _streamProvider;
        private readonly IStreamNamespaces _streamNamspaces;

        public RoomStatusUpdateBEConsumer([PersistentState("subscribeEndpointConfiguration", "subscribeEndpointConfigurationStore")] IPersistentState<SubscribeEndpoint> configuration, IClusterClient clusterClient, IStreamProvider streamProvider, IStreamNamespaces streamNamspaces)
        {
            _clusterClient = clusterClient;
            _streamProvider = streamProvider;
            _streamNamspaces = streamNamspaces;
            _configuration = configuration;
        }

        public override async Task OnActivateAsync()
        {
            await base.OnActivateAsync();
        }

        public Guid GetPrimaryKey()
        {
            var primaryKey = this.GetPrimaryKey(out string _);
            return primaryKey;
        }

        public Task<string> Namespace()
        {
            return Task.FromResult(_streamNamspaces.InboundNamespace<RoomStatusUpdateBERequestDto, Constants.Outbound.OperaCloud>());
        }

        public async Task SetConfiguration(ISubscribeEndpoint configuration)
        {
            _configuration.State = (SubscribeEndpoint)configuration;
            await _configuration.WriteStateAsync();
        }

        public async Task OnNextAsync(string content, StreamSequenceToken token) 
        {
            if (_configuration == null || string.IsNullOrEmpty(_configuration.State.Url))
            {
                throw new Exception("Hotel is not connected to the PMS. Please Connect first.");
            }

            var response = GetResponseContent(content);

            var stream = _streamProvider.GetStream<RoomStatusUpdate>(this.GetPrimaryKey(), HtngNamespaces.RoomStatusUpdateBENamespace);

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

        public Task OnCompletedAsync()
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
