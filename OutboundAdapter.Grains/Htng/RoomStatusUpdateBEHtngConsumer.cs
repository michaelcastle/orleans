using Orleans;
using Orleans.Runtime;
using Orleans.Streams;
using OutboundAdapter.Interfaces.Htng;
using OutboundAdapter.Interfaces.Models;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace LinkController.OperaCloud.Consumers.Inbound
{
    public class RoomStatusUpdateBEHtngConsumer : Grain, IRoomStatusUpdateBEHtngConsumer
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IPersistentState<InboundConfiguration> _configuration;

        public RoomStatusUpdateBEHtngConsumer([PersistentState("subscribeEndpointConfiguration", "subscribeEndpointConfigurationStore")] IPersistentState<InboundConfiguration> configuration,  IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        public override async Task OnActivateAsync()
        {

            await base.OnActivateAsync();
        }

        public async Task SetConfiguration(InboundConfiguration configuration)
        {
            _configuration.State = configuration;
            await _configuration.WriteStateAsync();
        }

        public async Task OnNextAsync(RoomStatusUpdate content, StreamSequenceToken token)
        {
            if (_configuration == null || string.IsNullOrEmpty(_configuration.State.Url))
            {
                throw new Exception("Hotel is not connected to the PMS. Please Connect first.");
            }

            using (var client = _httpClientFactory.CreateClient())
            {
                client.BaseAddress = new Uri(_configuration.State.Url);
                var result = await client.PostAsync(_configuration.State.Endpoint, new StringContent(JsonSerializer.Serialize(content), Encoding.UTF8, _configuration.State.MediaType)); // StringContent sets the Content-Type header
                result.EnsureSuccessStatusCode();
            }
        }

        Task IAsyncObserver<RoomStatusUpdate>.OnCompletedAsync()
        {
            throw new NotImplementedException();
        }

        public Task OnErrorAsync(Exception ex)
        {
            throw new NotImplementedException();
        }
    }
}
