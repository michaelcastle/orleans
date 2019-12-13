using ServiceExtensions.PmsAdapter.ClientChannel;
using ServiceExtensions.PmsAdapter.Connected_Services.PmsProcessor;

namespace ServiceExtensions.PmsAdapter.SignIn.Authentication
{
    public interface ISecurityAuthenticator
    {
        bool Validate(IClientChannelFactory<IPMSInterfaceContractChannel> clientFactory, string username, string password);
    }
}
