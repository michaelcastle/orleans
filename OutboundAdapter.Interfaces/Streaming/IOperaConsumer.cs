using Orleans;
using Orleans.Streams;
using OutboundAdapter.Interfaces.Models;

namespace OutboundAdapter.Interfaces.Streaming
{
    public interface IOperaConsumer : IAsyncObserver<UpdateRoomStatusRequest>, IGrainWithIntegerKey
    {
    }
}
