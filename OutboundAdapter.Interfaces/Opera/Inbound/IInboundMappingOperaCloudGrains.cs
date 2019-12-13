using OutboundAdapter.Interfaces.Opera.Models;
using System.Threading.Tasks;

namespace OutboundAdapter.Interfaces.Opera
{
    public interface IInboundMappingOperaCloudGrains : Orleans.IGrainWithIntegerKey
    {
        Task<RoomStatusUpdateBERequestDto> MapRoomStatusUpdateBE(string message);
        Task<UpdateRoomStatusResponseBodyDto> MapUpdateRoomStatusResponse(string message);
    }
}
