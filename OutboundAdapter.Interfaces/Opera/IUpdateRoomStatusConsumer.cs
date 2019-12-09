using Orleans;
using Orleans.Streams;
using OutboundAdapter.Interfaces.Models;

namespace OutboundAdapter.Interfaces.Opera
{
    public interface IUpdateRoomStatusConsumer : IAsyncObserver<UpdateRoomStatus>, IGrainWithIntegerKey
    {
    }
}
