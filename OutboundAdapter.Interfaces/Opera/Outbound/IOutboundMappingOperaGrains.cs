using OutboundAdapter.Interfaces.Models;
using OutboundAdapter.Interfaces.Opera.Models;
using System.Threading.Tasks;

namespace OutboundAdapter.Interfaces.Opera
{
    public interface IOutboundMappingOperaGrains : Orleans.IGrainWithIntegerKey
    {
        Task<string> MapFetchProfile(FetchProfile request);
        Task<string> MapFetchReservation(FetchReservation request);
        Task<string> MapReservationLookup(ReservationLookup request);
        Task<string> MapUpdateRoomStatus(UpdateRoomStatusRequestDto request);
    }
}
