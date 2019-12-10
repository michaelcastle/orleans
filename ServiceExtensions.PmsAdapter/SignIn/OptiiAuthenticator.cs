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

        public bool Validate(string username, string password, string hotelId)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                return false;
            }

            // Login
            var userSession = _cachedLoginService.ExternalLogin(username, password, "External Login", hotelId);
            if (userSession != null && userSession.IsAuthorised && userSession.SessionId != Guid.Empty)
            {
                return userSession.IsAuthorised;
            }

            return false;
        }
    }
}
