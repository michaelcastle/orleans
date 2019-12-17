using Orleans;
using Orleans.Streams;
using OutboundAdapter.Interfaces.Models;

namespace LinkController.OperaCloud.Interfaces.Outbound
{
    public interface IUpdateRoomStatusConsumer : IAsyncObserver<UpdateRoomStatus>, IGrainWithIntegerKey
    {
    }
}
