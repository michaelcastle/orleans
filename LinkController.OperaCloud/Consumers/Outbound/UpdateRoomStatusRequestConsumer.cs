using LinkController.OperaCloud.Interfaces;
using LinkController.OperaCloud.Interfaces.Models;
using LinkController.OperaCloud.Interfaces.Outbound;
using Orleans;
using Orleans.Runtime;
using Orleans.Streams;
using OutboundAdapter.Interfaces;
using OutboundAdapter.Interfaces.Models;
using OutboundAdapter.Interfaces.StreamHelpers;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace LinkController.OperaCloud.Consumers.Outbound
{
    public class UpdateRoomStatusRequestConsumer : Grain, IGrainWithIntegerKey, IUpdateRoomStatusRequestOperaCloudConsumer
    {
        private readonly List<IAsyncStream<UpdateRoomStatusRequestDto>> _streams = new List<IAsyncStream<UpdateRoomStatusRequestDto>>();
        private readonly List<StreamSubscriptionHandle<UpdateRoomStatusRequestDto>> _handles = new List<StreamSubscriptionHandle<UpdateRoomStatusRequestDto>>();
        private readonly IHttpClientFactory _httpClientFactory;
        private IHotelPmsGrain _hotel;
        private IStreamProvider _streamProvider;
        private readonly IStreamNamespaces _streamNamspaces;
        private IAsyncObservable<UpdateRoomStatusRequestDto> consumer;
        private StreamSubscriptionHandle<UpdateRoomStatusRequestDto> consumerHandle;
        private readonly IPersistentState<SubscribeEndpoint> _configuration;

        public UpdateRoomStatusRequestConsumer([PersistentState("subscribeEndpointConfiguration", "subscribeEndpointConfigurationStore")] IPersistentState<SubscribeEndpoint> configuration, IHttpClientFactory httpClientFactory, IStreamNamespaces streamNamspaces)
        {
            _httpClientFactory = httpClientFactory;
            _streamNamspaces = streamNamspaces;
            _configuration = configuration;
        }

        public override async Task OnActivateAsync()
        {
            _streamProvider = GetStreamProvider("SMSProvider");

            _hotel = GrainFactory.GetGrain<IHotelPmsGrain>((int)this.GetPrimaryKeyLong());

            var stream = _streamProvider.GetStream<UpdateRoomStatusRequestDto>(this.GetPrimaryKey(), Constants.Outbound.OperaCloud.UpdateRoomStatusRequestStream);
            _streams.Add(stream);
            _handles.Add(await stream.SubscribeAsync(OnNextAsync, OnErrorAsync, OnCompletedAsync));

            await base.OnActivateAsync();
        }

        public Task<string> Namespace()
        {
            return Task.FromResult(Constants.Outbound.OperaCloud.UpdateRoomStatusRequestStream);
        }

        public Task OnCompletedAsync()
        {
            return Task.CompletedTask;
        }

        public Task OnErrorAsync(Exception ex)
        {
            Console.WriteLine($"==== ERROR: {ex.Message}", ConsoleColor.DarkRed);
            return Task.CompletedTask;
        }

        public async Task OnNextAsync(UpdateRoomStatusRequestDto request, StreamSequenceToken token = null)
        {
            var response = await PostAsync(request);
            var resultStream = await SubmitResponse(response);

            Console.WriteLine(resultStream);
        }

        private async Task<HttpResponseMessage> PostAsync(UpdateRoomStatusRequestDto request)
        {
            var content = MapContent(request);
            var client = _httpClientFactory.CreateClient(nameof(Constants.Outbound.OperaCloud));
            
            client.BaseAddress = new Uri(_configuration.State.Url);
            client.DefaultRequestHeaders.Add("SOAPAction", "http://webservices.micros.com/htng/2008B/SingleGuestItinerary#UpdateRoomStatus");

            var response = await client.PostAsync(_configuration.State.Endpoint, await content); // StringContent sets the Content-Type header
            response.EnsureSuccessStatusCode();
            return response;
        }

        private async Task<StringContent> MapContent(UpdateRoomStatusRequestDto request)
        {
            var mapper = GrainFactory.GetGrain<IOutboundMappingOperaCloudGrains>((int)this.GetPrimaryKeyLong());
            var content = await mapper.MapUpdateRoomStatus(request);
            return new StringContent(content, Encoding.UTF8, "text/xml");
        }

        private async Task<string> SubmitResponse(HttpResponseMessage response)
        {
            var resultStream = response.Content.ReadAsStringAsync();
            var streamNamespace = _streamNamspaces.OutboundNamespace<UpdateRoomStatusResponseEnvelopeDto, Constants.Outbound.OperaCloud>();
            var stream = _streamProvider.GetStream<string>(this.GetPrimaryKey(), streamNamespace);

            var streamed = stream.OnNextAsync(await resultStream);
            await streamed;
            if (streamed.IsFaulted)
            {
                throw new Exception("Stream failed");
            }

            return resultStream.Result;
        }

        public async Task SetConfiguration(ISubscribeEndpoint configuration)
        {
            _configuration.State = (SubscribeEndpoint)configuration;
            await _configuration.WriteStateAsync();
        }

        public async Task BecomeConsumer(Guid streamId, string streamNamespace, string providerToUse)
        {
            var streamProvider = GetStreamProvider(providerToUse);
            consumer = streamProvider.GetStream<UpdateRoomStatusRequestDto>(streamId, streamNamespace);
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
