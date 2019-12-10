using System.Threading.Tasks;

namespace ServiceExtensions.PmsAdapter.SubmitMessage
{
    public interface ISubmitMessageHandler
    {
        Task<SubmitMessageResponse> Submit(SubmitMessage submitMessage);
    }
}
