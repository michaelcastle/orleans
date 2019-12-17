using LinkController.OperaCloud.Interfaces.Models;
using OutboundAdapter.Interfaces.Models;
using System.Threading.Tasks;

namespace LinkController.OperaCloud.Interfaces.Outbound
{
    public interface IOutboundMappingOperaGrains : Orleans.IGrainWithIntegerKey
    {
        Task<string> MapFetchProfile(FetchProfile request);
        Task<string> MapFetchReservation(FetchReservation request);
        Task<string> MapReservationLookup(ReservationLookup request);
        Task<string> MapUpdateRoomStatus(UpdateRoomStatusRequestDto request);
    }
}
