using OutboundAdapter.Interfaces.Models;
using System.Threading.Tasks;

namespace OutboundAdapter.Interfaces
{
    public interface IInboundAdapterGrain : Orleans.IGrainWithIntegerKey
    {
        Task<OrderItem> QueueRoom(int hotelId, int number);
        Task<OrderItem> GuestStatusNotification(int hotelId, int number);
        Task<OrderItem> NewProfile(int hotelId, int number);
        Task<OrderItem> UpdateProfile(int hotelId, int number);
        Task<OrderItem> RoomStatusUpdate(int number, RoomStatusUpdate content);
    }
}
