using ServiceExtensions.PmsAdapter.ClientChannel;
using ServiceExtensions.PmsAdapter.Connected_Services.PmsProcessor;
using ServiceExtensions.PmsAdapter.SignIn.Authentication;
using ServiceExtensions.PmsAdapter.SignIn.CachedLogin;
using System;
using System.Threading.Tasks;

namespace ServiceExtensions.PmsAdapter.SignIn
{
    public class OptiiAuthenticator : ISecurityAuthenticator
    {
        private readonly ICachedExternalLogin _cachedLoginService;

        public OptiiAuthenticator(ICachedExternalLogin cachedLoginService)
        {
            _cachedLoginService = cachedLoginService;
        }

        public async Task<bool> Validate(IClientChannelFactory<IPMSInterfaceContractChannel> clientFactory, string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                return false;
            }

            // Login
            var userSession = await _cachedLoginService.ExternalLogin(clientFactory, username, password);
            if (userSession != null && userSession.IsAuthorised && userSession.SessionId != Guid.Empty)
            {
                return userSession.IsAuthorised;
            }

            return false;
        }
    }
}
