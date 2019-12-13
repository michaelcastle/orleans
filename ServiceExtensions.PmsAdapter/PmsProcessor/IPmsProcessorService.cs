using ServiceExtensions.PmsAdapter.ClientChannel;
using ServiceExtensions.PmsAdapter.Connected_Services.PmsProcessor;
using System.Threading.Tasks;

namespace ServiceExtensions.PmsAdapter.PmsProcessor
{
    public interface IPmsProcessorService
    {
        Task<bool> SubmitMessage(IClientChannelFactory<IPMSInterfaceContractChannel> clientFactory, string username, string password, string messageString);
    }
}
