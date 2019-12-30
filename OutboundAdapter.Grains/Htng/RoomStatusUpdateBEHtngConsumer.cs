using Orleans;
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
        private InboundConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;
        public RoomStatusUpdateBEHtngConsumer(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public override async Task OnActivateAsync()
        {

            await base.OnActivateAsync();
        }

        public Task SetConfiguration(InboundConfiguration configuration)
        {
            _configuration = configuration;
            //await _configuration.WriteStateAsync();

            return Task.CompletedTask;
        }

        public async Task OnNextAsync(RoomStatusUpdate content, StreamSequenceToken token)
        {
            if (_configuration == null || string.IsNullOrEmpty(_configuration.Url))
            {
                throw new Exception("Hotel is not connected to the PMS. Please Connect first.");
            }

            using (var client = _httpClientFactory.CreateClient())
            {
                client.BaseAddress = new Uri(_configuration.Url);
                var result = await client.PostAsync(_configuration.Endpoint, new StringContent(JsonSerializer.Serialize(content), Encoding.UTF8, _configuration.MediaType)); // StringContent sets the Content-Type header
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
