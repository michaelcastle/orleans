using LinkController.OperaCloud.Interfaces.Models;
using Orleans;
using Orleans.Streams;
using OutboundAdapter.Interfaces;

namespace LinkController.OperaCloud.Interfaces.Outbound
{
    public interface IUpdateRoomStatusRequestOperaCloudConsumer : IAsyncObserver<UpdateRoomStatusRequestDto>, ISubscribeWithNamespaceObserver
    {
    }
}
