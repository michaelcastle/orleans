namespace ServiceExtensions.PmsAdapter.SignIn.CachedLogin
{
    public interface ICachedExternalLogin
    {
        SessionItem ExternalLoginNoCache(string username, string password, string lastAction, string hotelId);
        SessionItem ExternalLogin(string username, string password, string lastAction, string hotelId);
        void ClearCache(string username, string hotelId);
    }
}
