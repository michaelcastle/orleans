using ServiceExtensions.PmsAdapter.ClientChannel;
using ServiceExtensions.PmsAdapter.Connected_Services.PmsProcessor;
using System.Threading.Tasks;

namespace ServiceExtensions.PmsAdapter.SignIn
{
    public interface ISessionItemAuthenticationService
    {
        Task<SessionItem> SignIn(IClientChannelFactory<IPMSInterfaceContractChannel> _clientFactory, string username, string password);
    }
}
