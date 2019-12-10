using Orleans;
using Orleans.Streams;

namespace OutboundAdapter.Interfaces.Opera.Inbound
{
    public interface IRoomStatusUpdateConsumer : IAsyncObserver<string>, IGrainWithIntegerKey
    {
    }
}
