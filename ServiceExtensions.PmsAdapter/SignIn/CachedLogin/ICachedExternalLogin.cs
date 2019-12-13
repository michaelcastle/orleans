using ServiceExtensions.PmsAdapter.ClientChannel;
using ServiceExtensions.PmsAdapter.Connected_Services.PmsProcessor;

namespace ServiceExtensions.PmsAdapter.SignIn.CachedLogin
{
    public interface ICachedExternalLogin
    {
        SessionItem ExternalLoginNoCache(IClientChannelFactory<IPMSInterfaceContractChannel> _clientFactory, string username, string password);
        SessionItem ExternalLogin(IClientChannelFactory<IPMSInterfaceContractChannel> _clientFactory, string username, string password);
        void ClearCache(IClientChannelFactory<IPMSInterfaceContractChannel> _clientFactory, string username);
    }
}
