using Orleans;
using Orleans.Streams;

namespace LinkController.OperaCloud.Interfaces.Outbound.Inbound
{
    public interface IRoomStatusUpdateConsumer : IAsyncObserver<string>, IGrainWithIntegerKey
    {
    }
}
