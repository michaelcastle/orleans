using ServiceExtensions.PmsAdapter.SubmitMessage;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ServiceExtensions.Orleans
{
    public interface ISubmitMessageHandlerOracleCloud
    {
        Task<SubmitMessageResponse> SubmitRoomStatusUpdateBE(SubmitMessage submit);
        Task<SubmitMessageResponse> SubmitQueueRoomBE(SubmitMessage submit);
        Task<SubmitMessageResponse> SubmitGuestStatusNotificationExt(SubmitMessage submit);
        Task<SubmitMessageResponse> SubmitNewProfile(SubmitMessage submit);
        Task<SubmitMessageResponse> SubmitUpdateProfile(SubmitMessage submit);
    }
}
