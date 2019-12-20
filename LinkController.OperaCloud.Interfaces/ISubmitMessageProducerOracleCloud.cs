using Orleans;
using ServiceExtensions.PmsAdapter.SubmitMessage;
using System.Threading.Tasks;

namespace LinkController.OperaCloud.Interfaces
{
    public interface ISubmitMessageProducerOracleCloud : IGrainWithIntegerKey
    {
        Task<SubmitMessageResponse> ProduceRoomStatusUpdateBE(string body);
        Task<SubmitMessageResponse> ProduceQueueRoomBE(string body);
        Task<SubmitMessageResponse> ProduceGuestStatusNotificationExt(string body);
        Task<SubmitMessageResponse> ProduceNewProfile(string body);
        Task<SubmitMessageResponse> ProduceUpdateProfile(string body);
    }
}
