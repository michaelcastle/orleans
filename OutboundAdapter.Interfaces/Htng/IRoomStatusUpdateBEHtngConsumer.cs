using Orleans;
using Orleans.Streams;
using OutboundAdapter.Interfaces.Models;

namespace OutboundAdapter.Interfaces.Htng
{
    public interface IRoomStatusUpdateBEHtngConsumer : IAsyncObserver<RoomStatusUpdate>, IGrainWithIntegerKey
    {
    }
}
