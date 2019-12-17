using Orleans;
using Orleans.Streams;
using OutboundAdapter.Interfaces;
using OutboundAdapter.Interfaces.Consumer;
using OutboundAdapter.Interfaces.Models;
using OutboundAdapter.Interfaces.Opera;
using OutboundAdapter.Interfaces.Opera.Inbound;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace OutboundAdapter.Grains.Opera
{
    //[ImplicitStreamSubscription(Constants.Inbound.V2.RoomStatusUpdateStream)]
    public class RoomStatusUpdateConsumer : Grain, IGrainWithIntegerKey, IRoomStatusUpdateConsumer, ISubmitMessageApiConsumer
    {
        //private readonly List<IAsyncStream<string>> _streams = new List<IAsyncStream<string>>();
        //private readonly List<StreamSubscriptionHandle<string>> _handles = new List<StreamSubscriptionHandle<string>>();
        //private IStreamProvider _streamProvider;
        private readonly IClusterClient _clusterClient;
        private InboundConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;
        private IAsyncObservable<string> consumer;
        private StreamSubscriptionHandle<string> consumerHandle;

        public RoomStatusUpdateConsumer(IClusterClient clusterClient, IHttpClientFactory httpClientFactory)
        {
            _clusterClient = clusterClient;
            _httpClientFactory = httpClientFactory;
        }

        public override async Task OnActivateAsync()
        {
            //_streamProvider = GetStreamProvider("SMSProvider");

            //var stream = _streamProvider.GetStream<string>(this.GetPrimaryKey(), Constants.Inbound.V2.RoomStatusUpdateStream);
            //_streams.Add(stream);
            //_handles.Add(await stream.SubscribeAsync(OnNextAsync, OnErrorAsync, OnCompletedAsync));

            await base.OnActivateAsync();
        }

        //public Task OnCompletedAsync()  
        //{
        //    throw new NotImplementedException();
        //}

        //public Task OnErrorAsync(Exception ex)
        //{
        //    throw new NotImplementedException();
        //}

        //public async Task OnNextAsync(string content, StreamSequenceToken token = null) 
        //{
        //    var hotel = _clusterClient.GetGrain<IHotelPmsGrain>(HotelId);
        //    if (!await hotel.IsInboundConnected())
        //    {
        //        throw new Exception("Hotel is not connected to the PMS. Please Connect first.");
        //    }

        //    var mapper = _clusterClient.GetGrain<IInboundMappingGrains>(HotelId);
        //    var request = mapper.MapRoomStatusUpdateBE(content);

        //    var hotelGrain = _clusterClient.GetGrain<IInboundAdapterGrain>(HotelId);
        //    var requestDto = await request;
        //    var response = hotelGrain.RoomStatusUpdate(1, new RoomStatusUpdate
        //    {
        //        ResortId = requestDto.ResortId,
        //        RoomNumber = requestDto.RoomNumber,
        //        RoomStatus = requestDto.RoomStatus,
        //        RoomType = requestDto.RoomType
        //    });

        //    // TODO: Send response to Optii, however that is done.
        //}

        Task ISubscribeToResponseObserver.SetConfiguration(InboundConfiguration configuration)
        {
            _configuration = configuration;
            //await _configuration.WriteStateAsync();

            return Task.CompletedTask;
        }

        public async Task OnNextAsync(string content, StreamSequenceToken token)
        {
            if (_configuration == null || string.IsNullOrEmpty(_configuration.Url))
            {
                throw new Exception("Hotel is not connected to the PMS. Please Connect first.");
            }

            var response =  GetResponseContent(content);

            using (var client = _httpClientFactory.CreateClient())
            {
                client.BaseAddress = new Uri(_configuration.Url);
                var result = await client.PostAsync(_configuration.Endpoint, new StringContent(JsonSerializer.Serialize(await response), Encoding.UTF8, _configuration.MediaType)); // StringContent sets the Content-Type header
                result.EnsureSuccessStatusCode();
            }
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
