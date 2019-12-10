using Microsoft.Extensions.Caching.Memory;
using System;

namespace ServiceExtensions.PmsAdapter.SignIn.CachedLogin
{
    public class OptiiLoginCache : ILoginCacheService 
    {
        private readonly IMemoryCache _memoryCache;

        public static string UserSessionPrefix => "UserSession";

        public OptiiLoginCache(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public void ClearCache(string username, string hotelId)
        {
            // can use "" for the password since it will clear anything which starts with the key given
            var cacheKey = GetCacheKey(username, hotelId);
            _memoryCache.Remove(cacheKey);
        }

        public string GetCacheKey(string username, string hotelId)
        {
            return $"{UserSessionPrefix}_{username}_{hotelId}";
        }

        public void UpdateCache(ICachedSessionItem session, string username, string password, string hotelId)
        {
            // Only cache a login which is authorised so if someone is not authenticated it can retry and potentially work if the password was updated/session expired etc.
            var cacheKey = GetCacheKey(username, hotelId);
            var memoryCacheOption = new MemoryCacheEntryOptions()
            {
                SlidingExpiration = new TimeSpan(0, 30, 0)
            };

            _memoryCache.Set(cacheKey, session, memoryCacheOption);
        }

        public bool TryGetValue(string username, string password, string hotelId, out ICachedSessionItem session)
        {
            session = null;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                return false;
            }

            var key = GetCacheKey(username, hotelId);
            var isFound = _memoryCache.TryGetValue(key, out session);
            if (!isFound)
            {
                return false;
            }

            var isCredentialsEqual = session.CredentialsEqual(username, password);
            if (!isCredentialsEqual)
            {
                ClearCache(username, hotelId);
            }

            return isCredentialsEqual;

        }
    }
}
