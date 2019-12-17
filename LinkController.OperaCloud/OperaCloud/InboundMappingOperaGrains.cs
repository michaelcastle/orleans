using LinkController.OperaCloud;
using LinkController.OperaCloud.Outbound.Inbound;
using LinkController.OperaCloud.Outbound.Models;
using Orleans;
using Orleans.Concurrency;
using System.Threading.Tasks;

namespace LinkController.OperaCloud.OperaCloud
{
    [StatelessWorker]
    public class InboundMappingOperaGrains : Grain, IInboundMappingOperaCloudGrains
    {
        private readonly IOperaEnvelopeSerializer _operaRequestSerializer;

        public InboundMappingOperaGrains(IOperaEnvelopeSerializer operaRequestSerializer)
        {
            _operaRequestSerializer = operaRequestSerializer;
        }

        async Task<RoomStatusUpdateBERequestDto> IInboundMappingOperaCloudGrains.MapRoomStatusUpdateBE(string message)
        {
            return await Task.Run(() => {
                return _operaRequestSerializer.DeserialiseNode<RoomStatusUpdateBERequestDto>(message, "RoomStatusUpdateBERequest");
            });
        }

        async Task<UpdateRoomStatusResponseBodyDto> IInboundMappingOperaCloudGrains.MapUpdateRoomStatusResponse(string message)
        {
            return await Task.Run(() => {
                return _operaRequestSerializer.DeserialiseNode<UpdateRoomStatusResponseBodyDto>(message, "RoomStatusUpdateBEResponse");
            });
        }
    }
}
