using Orleans;
using Orleans.Streams;

namespace LinkController.OperaCloud.Interfaces.Outbound
{
    public interface IUpdateRoomStatusResponseOperaCloudConsumer : IAsyncObserver<string>, IGrainWithIntegerKey
    {
    }
}
