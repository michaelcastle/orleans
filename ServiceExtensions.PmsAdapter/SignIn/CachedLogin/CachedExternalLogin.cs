using ServiceExtensions.PmsAdapter.ClientChannel;
using ServiceExtensions.PmsAdapter.Connected_Services.PmsProcessor;

namespace ServiceExtensions.PmsAdapter.SignIn.CachedLogin
{
    public class CachedExternalLogin : ICachedExternalLogin
    {
        private readonly ISessionItemAuthenticationService _userAuthenticationService;
        private readonly ILoginCacheService _loginCacheService;

        public CachedExternalLogin(ISessionItemAuthenticationService userAuthenticationService, ILoginCacheService loginCacheService)
        {
            _userAuthenticationService = userAuthenticationService;
            _loginCacheService = loginCacheService;
        }

        public SessionItem ExternalLogin(IClientChannelFactory<IPMSInterfaceContractChannel> clientFactory, string username, string password)
        {
            var isFound = _loginCacheService.TryGetValue(clientFactory, username, password, out ICachedSessionItem cachedSession);
            if (isFound)
            {
                return (SessionItem)cachedSession;
            }

            var userSessionDto = _userAuthenticationService.SignIn(clientFactory, username, password);
            var session = new SessionItem(username, password, userSessionDto);
            if (session.IsAuthorised)
            {
                _loginCacheService.UpdateCache(clientFactory, session, username, password);
            }
            else
            {
                _loginCacheService.ClearCache(clientFactory, username);
            }
            return session;
        }

        public void ClearCache(IClientChannelFactory<IPMSInterfaceContractChannel> clientFactory, string username)
        {
            _loginCacheService.ClearCache(clientFactory, username);
        }

        public SessionItem ExternalLoginNoCache(IClientChannelFactory<IPMSInterfaceContractChannel> clientFactory, string username, string password)
        {
            _loginCacheService.ClearCache(clientFactory, username);
            return ExternalLogin(clientFactory, username, password);
        }
    }
}
