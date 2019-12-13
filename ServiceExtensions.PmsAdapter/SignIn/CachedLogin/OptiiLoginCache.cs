using Microsoft.Extensions.Caching.Memory;
using ServiceExtensions.PmsAdapter.ClientChannel;
using ServiceExtensions.PmsAdapter.Connected_Services.PmsProcessor;
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

        public void ClearCache(IClientChannelFactory<IPMSInterfaceContractChannel> clientFactory, string username)
        {
            // can use "" for the password since it will clear anything which starts with the key given
            var cacheKey = GetCacheKey(clientFactory, username);
            _memoryCache.Remove(cacheKey);
        }

        public string GetCacheKey(IClientChannelFactory<IPMSInterfaceContractChannel> clientFactory, string username)
        {
            return $"{UserSessionPrefix}_{username}_{clientFactory.EndPoint.Address.Uri.AbsoluteUri}";
        }

        public void UpdateCache(IClientChannelFactory<IPMSInterfaceContractChannel> clientFactory, ICachedSessionItem session, string username, string password)
        {
            // Only cache a login which is authorised so if someone is not authenticated it can retry and potentially work if the password was updated/session expired etc.
            var cacheKey = GetCacheKey(clientFactory, username);
            var memoryCacheOption = new MemoryCacheEntryOptions()
            {
                SlidingExpiration = new TimeSpan(0, 30, 0)
            };

            _memoryCache.Set(cacheKey, session, memoryCacheOption);
        }

        public bool TryGetValue(IClientChannelFactory<IPMSInterfaceContractChannel> clientFactory, string username, string password, out ICachedSessionItem session)
        {
            session = null;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                return false;
            }

            var key = GetCacheKey(clientFactory, username);
            var isFound = _memoryCache.TryGetValue(key, out session);
            if (!isFound)
            {
                return false;
            }

            var isCredentialsEqual = session.CredentialsEqual(username, password);
            if (!isCredentialsEqual)
            {
                ClearCache(clientFactory, username);
            }

            return isCredentialsEqual;

        }
    }
}
