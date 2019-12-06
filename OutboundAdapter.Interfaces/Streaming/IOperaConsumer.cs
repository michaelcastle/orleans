using Orleans;
using Orleans.Streams;

namespace OutboundAdapter.Interfaces.Streaming
{
    public interface IOperaConsumer : IAsyncObserver<string>, IGrainWithIntegerKey
    {
    }
}
