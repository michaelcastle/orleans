using LinkController.OperaCloud.Interfaces;
using LinkController.OperaCloud.Interfaces.Models;
using LinkController.OperaCloud.Interfaces.Outbound.Inbound;
using Orleans;
using Orleans.Concurrency;
using System.Threading.Tasks;

namespace LinkController.OperaCloud.Grains.Mapping
{
    [StatelessWorker]
    public class InboundMappingOperaCloudGrains : Grain, IInboundMappingOperaCloudGrains
    {
        private readonly IOperaCloudEnvelopeSerializer _operaRequestSerializer;

        public InboundMappingOperaCloudGrains(IOperaCloudEnvelopeSerializer operaRequestSerializer)
        {
            _operaRequestSerializer = operaRequestSerializer;
        }

        async Task<RoomStatusUpdateBERequestDto> IInboundMappingOperaCloudGrains.MapRoomStatusUpdateBE(string message)
        {
            return await Task.Run(() =>
            {
                return _operaRequestSerializer.DeserialiseNode<RoomStatusUpdateBERequestDto>(message, "RoomStatusUpdateBERequest");
            });
        }

        async Task<UpdateRoomStatusResponseBodyDto> IInboundMappingOperaCloudGrains.MapUpdateRoomStatusResponse(string message)
        {
            return await Task.Run(() =>
            {
                return _operaRequestSerializer.DeserialiseNode<UpdateRoomStatusResponseBodyDto>(message, "RoomStatusUpdateBEResponse");
            });
        }
    }
}
