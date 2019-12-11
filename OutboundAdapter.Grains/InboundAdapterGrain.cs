using Orleans;
using OutboundAdapter.Interfaces;
using OutboundAdapter.Interfaces.Models;
using System;
using System.Threading.Tasks;

namespace OutboundAdapter.Grains
{
    public class InboundAdapterGrain : Grain, IInboundAdapterGrain
    {
        Task<OrderItem> IInboundAdapterGrain.GuestStatusNotification(int hotelId, int number)
        {
            throw new NotImplementedException();
        }

        Task<OrderItem> IInboundAdapterGrain.NewProfile(int hotelId, int number)
        {
            throw new NotImplementedException();
        }

        Task<OrderItem> IInboundAdapterGrain.QueueRoom(int hotelId, int number)
        {
            throw new NotImplementedException();
        }

        Task<OrderItem> IInboundAdapterGrain.RoomStatusUpdate(int number, RoomStatusUpdate content)
        {
            Console.WriteLine(number);
            return Task.FromResult(new OrderItem
            {
                HotelId = (int)this.GetPrimaryKeyLong(),
                Number = number
            });
        }

        Task<OrderItem> IInboundAdapterGrain.UpdateProfile(int hotelId, int number)
        {
            throw new NotImplementedException();
        }
    }
}
