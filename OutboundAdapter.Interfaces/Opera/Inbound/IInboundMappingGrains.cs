using OutboundAdapter.Interfaces.Opera.Models;
using System.Threading.Tasks;

namespace OutboundAdapter.Interfaces.Opera
{
    public interface IInboundMappingGrains : Orleans.IGrainWithIntegerKey
    {
        Task<RoomStatusUpdateBERequestDto> MapRoomStatusUpdateBE(string message);
        Task<UpdateRoomStatusResponseBodyDto> MapUpdateRoomStatusResponse(string message);
    }
}
