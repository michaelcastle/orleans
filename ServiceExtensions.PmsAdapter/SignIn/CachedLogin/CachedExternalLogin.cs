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

        public SessionItem ExternalLogin(string username, string password, string lastAction, string hotelId)
        {
            var isFound = _loginCacheService.TryGetValue(username, password, hotelId, out ICachedSessionItem cachedSession);
            if (isFound)
            {
                return (SessionItem)cachedSession;
            }

            var userSessionDto = _userAuthenticationService.SignIn(username, password, lastAction, hotelId);
            var session = new SessionItem(username, password, userSessionDto);
            if (session.IsAuthorised)
            {
                _loginCacheService.UpdateCache(session, username, password, hotelId);
            }
            else
            {
                _loginCacheService.ClearCache(username, hotelId);
            }
            return session;
        }

        public void ClearCache(string username, string hotelId)
        {
            _loginCacheService.ClearCache(username, hotelId);
        }

        public SessionItem ExternalLoginNoCache(string username, string password, string lastAction, string hotelId)
        {
            _loginCacheService.ClearCache(username, hotelId);
            return ExternalLogin(username, password, lastAction, hotelId);
        }
    }
}
