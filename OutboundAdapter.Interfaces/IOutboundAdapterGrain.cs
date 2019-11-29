using OutboundAdapter.Interfaces.Models;
using System.Threading.Tasks;

namespace OutboundAdapter.Interfaces
{
    public interface IOutboundAdapterGrain : Orleans.IGrainWithIntegerKey
    {
        Task<OrderItem> RemoteRequest(int hotelId, int number);
        Task<OrderItem> FetchProfile(int hotelId, int number);
        Task<OrderItem> FetchReservation(int hotelId, int number);
        Task<OrderItem> ReservationLookup(int hotelId, int number);
        Task<OrderItem> UpdateRoomStatus(int hotelId, int number);
    }
}
