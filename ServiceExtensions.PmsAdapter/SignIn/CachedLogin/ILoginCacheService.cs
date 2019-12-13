using ServiceExtensions.PmsAdapter.ClientChannel;
using ServiceExtensions.PmsAdapter.Connected_Services.PmsProcessor;

namespace ServiceExtensions.PmsAdapter.SignIn.CachedLogin
{
    public interface ILoginCacheService
    {
        void ClearCache(IClientChannelFactory<IPMSInterfaceContractChannel> clientFactory, string username);
        void UpdateCache(IClientChannelFactory<IPMSInterfaceContractChannel> clientFactory, ICachedSessionItem userSession, string username, string password);
        bool TryGetValue(IClientChannelFactory<IPMSInterfaceContractChannel> clientFactory, string username, string password, out ICachedSessionItem session);
    }
}
