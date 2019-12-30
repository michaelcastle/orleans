using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Streams;
using OutboundAdapter.Interfaces;
using OutboundAdapter.Interfaces.Models;
using OutboundAdapter.Interfaces.StreamHelpers;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace OutboundAdapter.Grains
{
    public class OutboundAdapterGrain : Grain, IOutboundAdapterGrain
    {
        private readonly ILogger logger;
        private IHotelPmsGrain _hotel;
        private IStreamProvider _streamProvider;
        //private StreamSequenceToken _streamSequenceToken;
        private static readonly SemaphoreSlim _semaphoreSlim = new SemaphoreSlim(1, 1);
        private readonly IStreamNamespaces _streamNamspaces;

        public OutboundAdapterGrain(ILogger<OutboundAdapterGrain> logger, IStreamNamespaces streamNamspaces)
        {
            this.logger = logger;
            _streamNamspaces = streamNamspaces;
        }

        public override async Task OnActivateAsync()
        {
            _streamProvider = GetStreamProvider("SMSProvider");

            _hotel = GrainFactory.GetGrain<IHotelPmsGrain>((int)this.GetPrimaryKeyLong());
            //_httpFactoryGrain = GrainFactory.GetGrain<IHttpFactoryGrain>(this.GetPrimaryKeyLong());

            await base.OnActivateAsync();
        }

        async Task<OrderItem> IOutboundAdapterGrain.FetchProfile(int hotelId, int number)
        {
            return await Process(hotelId, number);
        }

        async Task<OrderItem> IOutboundAdapterGrain.FetchReservation(int hotelId, int number)
        {
            return await Process(hotelId, number);
        }

        async Task<OrderItem> IOutboundAdapterGrain.ReservationLookup(int hotelId, int number)
        {
            return await Process(hotelId, number);
        }

        // Remove async in order to make sure those are processed in order and sychronous while the others are async
        async Task<OrderItem> IOutboundAdapterGrain.UpdateRoomStatus(int number, UpdateRoomStatus content)
        {
            await _semaphoreSlim.WaitAsync();
            try
            {
                //    var streamNamespace = ""; // _streamNamspaces.OutboundNamespace<UpdateRoomStatus, Constants.Outbound.OperaCloud>();
                //    var stream = _streamProvider.GetStream<UpdateRoomStatus>(this.GetPrimaryKey(), streamNamespace);
                //    var streamed = stream.OnNextAsync(content);
                //    await streamed;
                //    if (streamed.IsFaulted)
                //    {
                //        throw new Exception("Stream failed");
                //    }

                //    var configuration = await _hotel.GetOutboundConfiguration();
                var orderItem = new OrderItem
                {
                    PrimaryKey = this.GetPrimaryKey(),
                    HotelId = (int)this.GetPrimaryKeyLong(),
                    Number = number,
                    //Response = request,
                    //TotalNumber = configuration.TotalNumber
                };

                //    logger.LogDebug($"\n Message received: I am {orderItem.PrimaryKey}, number = '{orderItem.Number}, Response = '{orderItem.Response}'");

                return orderItem;
            }
            finally
            {
                //When the task is ready, release the semaphore. It is vital to ALWAYS release the semaphore when we are ready, or else we will end up with a Semaphore that is forever locked.
                //This is why it is important to do the Release within a try...finally clause; program execution may crash or take a different path, this way you are guaranteed execution
                _semaphoreSlim.Release();
            }
        }

        async Task<OrderItem> IOutboundAdapterGrain.RemoteRequest(int hotelId, int number)
        {
            return await Process(hotelId, number);
        }

        private Task<OrderItem> Process(int hotelId, int number)
        {
            var random = new Random();
            var randomTimeToProcess = random.Next(101, 1000);

            Thread.Sleep(randomTimeToProcess);
            //await Task.Delay(randomTimeToProcess);

            var orderItem = new OrderItem
            {
                PrimaryKey = this.GetPrimaryKey(),
                HotelId = hotelId,
                Number = number
            };

            logger.LogInformation($"\n Message received: I am {orderItem.PrimaryKey}, number = '{orderItem.Number}'");

            return Task.FromResult(orderItem);
        }
    }
}
