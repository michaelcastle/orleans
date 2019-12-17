using LinkController.OperaCloud.Interfaces.Models;
using System.Threading.Tasks;

namespace LinkController.OperaCloud.Interfaces.Outbound.Inbound
{
    public interface IInboundMappingOperaCloudGrains : Orleans.IGrainWithIntegerKey
    {
        Task<RoomStatusUpdateBERequestDto> MapRoomStatusUpdateBE(string message);
        Task<UpdateRoomStatusResponseBodyDto> MapUpdateRoomStatusResponse(string message);
    }
}
