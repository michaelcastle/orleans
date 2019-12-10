using ServiceExtensions.PmsAdapter.SignIn.Authentication;
using System;

namespace ServiceExtensions.PmsAdapter.SignIn.CachedLogin
{
    public class CachedSecurityAuthenticator : ISecurityAuthenticator
    {
        private readonly ICachedExternalLogin _cachedLoginService;

        public CachedSecurityAuthenticator(ICachedExternalLogin cachedLoginService)
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
