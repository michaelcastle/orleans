using System.Net.Http;
using System.Threading.Tasks;

namespace OutboundAdapter.Interfaces
{
    public interface IHttpFactoryGrain : Orleans.IGrainWithIntegerKey
    {
        Task<string> FetchProfile(string request);
        Task<string> FetchReservation(string request);
        Task<string> ReservationLookup(string request);
        Task<string> UpdateRoomStatus(string request);
    }
}
