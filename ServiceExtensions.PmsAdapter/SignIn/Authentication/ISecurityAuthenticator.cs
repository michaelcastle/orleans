using ServiceExtensions.PmsAdapter.ClientChannel;
using ServiceExtensions.PmsAdapter.Connected_Services.PmsProcessor;
using System.Threading.Tasks;

namespace ServiceExtensions.PmsAdapter.SignIn.Authentication
{
    public interface ISecurityAuthenticator
    {
        Task<bool> Validate(IClientChannelFactory<IPMSInterfaceContractChannel> clientFactory, string username, string password);
    }
}
