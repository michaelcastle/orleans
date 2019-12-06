using Orleans;
using Orleans.Streams;
using OutboundAdapter.Interfaces;
using OutboundAdapter.Interfaces.Models;
using OutboundAdapter.Interfaces.PmsClients;
using OutboundAdapter.Interfaces.Streaming;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace OutboundAdapter.Grains.Opera
{
    [ImplicitStreamSubscription("UpdateRoomStatusOpera")]
    public class OperaConsumer : Grain, IGrainWithIntegerKey, IOperaConsumer
    {
        private readonly List<IAsyncStream<string>> _streams = new List<IAsyncStream<string>>();
        private readonly List<StreamSubscriptionHandle<string>> _handles = new List<StreamSubscriptionHandle<string>>();
        private readonly IHttpClientFactory _httpClientFactory;
        private IHotelPmsGrain _hotel;
        private const string Endpoint = "/OPERA9OSB/opera/OperaHTNG_EXT2008BWebServices";

        public OperaConsumer(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public override async Task OnActivateAsync()
        {
            var streamProvider = GetStreamProvider("SMSProvider");

            _hotel = GrainFactory.GetGrain<IHotelPmsGrain>((int)this.GetPrimaryKeyLong());

            var stream = streamProvider.GetStream<string>(this.GetPrimaryKey(), "UpdateRoomStatusOpera");
            _streams.Add(stream);
            _handles.Add(await stream.SubscribeAsync(OnNextAsync, OnErrorAsync, OnCompletedAsync));

            await base.OnActivateAsync();
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

        public async Task OnNextAsync(string request, StreamSequenceToken token = null)
        {
            var mapper = GrainFactory.GetGrain<IOutboundMappingGrains>((int)this.GetPrimaryKeyLong());
            var contentHandler = mapper.MapUpdateRoomStatus(request);

            var client = _httpClientFactory.CreateClient("Opera");
            var config = await _hotel.GetOutboundConfiguration();
            client.BaseAddress = new Uri(config.Url);
            client.DefaultRequestHeaders.Add("SOAPAction", "http://webservices.micros.com/htng/2008B/SingleGuestItinerary#UpdateRoomStatus");

            var content = await contentHandler;
            var response = await client.PostAsync(Endpoint, new StringContent(content, Encoding.UTF8, "text/xml")); // StringContent sets the Content-Type header
            Console.WriteLine(await response.Content.ReadAsStringAsync());
        }

        public static Task<string> ProcessRequest(string content)
        {
            var random = new Random();
            var randomTimeToProcess = random.Next(1, 1000);

            System.Threading.Thread.Sleep(randomTimeToProcess);
            //Task.Delay(randomTimeToProcess);

            return Task.FromResult(content);
        }
    }
}
