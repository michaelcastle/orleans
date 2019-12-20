using LinkController.OperaCloud.Interfaces.Models;
using Orleans;
using Orleans.Streams;

namespace LinkController.OperaCloud.Interfaces.Outbound
{
    public interface IUpdateRoomStatusRequestOperaCloudConsumer : IAsyncObserver<UpdateRoomStatusRequestDto>, IGrainWithIntegerKey
    {
    }
}
