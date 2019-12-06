using OutboundAdapter.Interfaces.Models;
using System.Threading.Tasks;

namespace OutboundAdapter.Interfaces
{
    public interface IOutboundMappingGrains : Orleans.IGrainWithIntegerKey
    {
        Task<string> MapFetchProfile(FetchProfile request);
        Task<string> MapFetchReservation(FetchReservation request);
        Task<string> MapReservationLookup(ReservationLookup request);
        Task<string> MapUpdateRoomStatus(string request);
    }
}
