using ServiceExtensions.PmsAdapter.ClientChannel;
using ServiceExtensions.PmsAdapter.Connected_Services.PmsProcessor;
using ServiceExtensions.PmsAdapter.SignIn.Authentication;
using ServiceExtensions.PmsAdapter.SignIn.CachedLogin;
using System;

namespace ServiceExtensions.PmsAdapter.SignIn
{
    public class OptiiAuthenticator : ISecurityAuthenticator
    {
        private readonly ICachedExternalLogin _cachedLoginService;

        public OptiiAuthenticator(ICachedExternalLogin cachedLoginService)
        {
            _cachedLoginService = cachedLoginService;
        }

        public bool Validate(IClientChannelFactory<IPMSInterfaceContractChannel> clientFactory, string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                return false;
            }

            // Login
            var userSession = _cachedLoginService.ExternalLogin(clientFactory, username, password);
            if (userSession != null && userSession.IsAuthorised && userSession.SessionId != Guid.Empty)
            {
                return userSession.IsAuthorised;
            }

            return false;
        }
    }
}
