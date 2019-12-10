namespace ServiceExtensions.PmsAdapter.SignIn.CachedLogin
{
    public interface ILoginCacheService
    {
        void ClearCache(string username, string hotelId);
        void UpdateCache(ICachedSessionItem userSession, string username, string password, string hotelId);
        bool TryGetValue(string username, string password, string hotelId, out ICachedSessionItem session);
    }
}
