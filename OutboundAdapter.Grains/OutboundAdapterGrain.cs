using Microsoft.Extensions.Logging;
using Orleans;
using OutboundAdapter.Interfaces;
using OutboundAdapter.Interfaces.Models;
using OutboundAdapter.Interfaces.PmsClients;
using System;
using System.Threading.Tasks;

namespace OutboundAdapter.Grains
{
    public class OutboundAdapterGrain : Grain, IOutboundAdapterGrain
    {
        private readonly ILogger logger;
        private IHotelPmsGrain _hotel;
        private IOutboundMappingGrains _hotelOutboundMapper;
        private IHttpFactoryGrain _httpFactoryGrain;
        private readonly IOperaHTNG2008BServiceClient _htng2008bClient;
        private readonly IOperaHTNG_EXT2008BWebServicesClient _htng2008BExtClient;

        public OutboundAdapterGrain(ILogger<OutboundAdapterGrain> logger, IOperaHTNG2008BServiceClient htng2008bClient, IOperaHTNG_EXT2008BWebServicesClient htng2008BExtClient)
        {
            this.logger = logger;
            //_httpFactoryGrain = new HttpFactoryOperaGrain(htng2008bClient, htng2008BExtClient);
            _htng2008bClient = htng2008bClient;
            _htng2008BExtClient = htng2008BExtClient;
        }

        public override async Task OnActivateAsync()
        {
            _hotel = GrainFactory.GetGrain<IHotelPmsGrain>(this.GetPrimaryKeyLong());
            _hotelOutboundMapper = GrainFactory.GetGrain<IOutboundMappingGrains>(this.GetPrimaryKeyLong());
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
        Task<OrderItem> IOutboundAdapterGrain.UpdateRoomStatus(int number)
        {
            var sourceUpdateRoomStatus = new UpdateRoomStatusRequest
            {
                HotelId = (int)this.GetPrimaryKeyLong(),
                RoomNumber = number.ToString()
            };

            AsyncHelper.RunSync(() => _hotel.IncrementAsync());
            var request = AsyncHelper.RunSync(() => _hotelOutboundMapper.MapUpdateRoomStatus(sourceUpdateRoomStatus));
            var response = AsyncHelper.RunSync(() => _htng2008BExtClient.OperaHTNG_EXT2008BWebServices(request));
            var configuration = AsyncHelper.RunSync(() => _hotel.Get());

            var orderItem = new OrderItem
            {
                PrimaryKey = this.GetPrimaryKey(),
                HotelId = (int)this.GetPrimaryKeyLong(),
                Number = number,
                Response = response,
                TotalNumber = configuration.TotalNumber
            };

            logger.LogDebug($"\n Message received: I am {orderItem.PrimaryKey}, number = '{orderItem.Number}, Response = '{orderItem.Response}'");

            return Task.FromResult(orderItem);
        }

        async Task<OrderItem> IOutboundAdapterGrain.RemoteRequest(int hotelId, int number)
        {
            return await Process(hotelId, number);
        }

        private Task<OrderItem> Process(int hotelId, int number)
        {
            var random = new Random();
            var randomTimeToProcess = random.Next(101, 1000);

            System.Threading.Thread.Sleep(randomTimeToProcess);
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
