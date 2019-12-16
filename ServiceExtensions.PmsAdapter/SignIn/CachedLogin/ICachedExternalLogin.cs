using ServiceExtensions.PmsAdapter.ClientChannel;
using ServiceExtensions.PmsAdapter.Connected_Services.PmsProcessor;
using System.Threading.Tasks;

namespace ServiceExtensions.PmsAdapter.SignIn.CachedLogin
{
    public interface ICachedExternalLogin
    {
        Task<SessionItem> ExternalLoginNoCache(IClientChannelFactory<IPMSInterfaceContractChannel> _clientFactory, string username, string password);
        Task<SessionItem> ExternalLogin(IClientChannelFactory<IPMSInterfaceContractChannel> _clientFactory, string username, string password);
        void ClearCache(IClientChannelFactory<IPMSInterfaceContractChannel> _clientFactory, string username);
    }
}
