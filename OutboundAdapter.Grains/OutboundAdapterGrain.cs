using Microsoft.Extensions.Logging;
using Orleans;
using OutboundAdapter.Interfaces;
using OutboundAdapter.Interfaces.Models;
using System;
using System.Threading.Tasks;
using Orleans.Placement;
using Orleans.Runtime;

namespace OutboundAdapter.Grains
{
    [PreferLocalPlacement]
    public class OutboundAdapterGrain : Grain, IOutboundAdapterGrain
    {
        private readonly ILogger logger; 

        public OutboundAdapterGrain(ILogger<OutboundAdapterGrain> logger)
        {
            this.logger = logger;
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
        Task<OrderItem> IOutboundAdapterGrain.UpdateRoomStatus(int hotelId, int number)
        {
            var random = new Random();
            var randomTimeToProcess = random.Next(1, 1000);

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

        async Task<OrderItem> IOutboundAdapterGrain.RemoteRequest(int hotelId, int number)
        {
            return await Process(hotelId, number);
        }

        private async Task<OrderItem> Process(int hotelId, int number)
        {
            var random = new Random();
            var randomTimeToProcess = random.Next(1, 1000);

            System.Threading.Thread.Sleep(randomTimeToProcess);
            //await Task.Delay(randomTimeToProcess);

            var orderItem = new OrderItem
            {
                PrimaryKey = this.GetPrimaryKey(),
                HotelId = hotelId,
                Number = number
            };

            logger.LogInformation($"\n Message received: I am {orderItem.PrimaryKey}, number = '{orderItem.Number}'");

            return await Task.FromResult(orderItem);
        }
    }
}
