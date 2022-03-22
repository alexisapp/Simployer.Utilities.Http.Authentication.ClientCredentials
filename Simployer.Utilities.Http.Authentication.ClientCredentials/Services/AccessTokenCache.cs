using Simployer.Utilities.Http.Authentication.ClientCredentials.Configuration;
using Simployer.Utilities.Http.Authentication.ClientCredentials.Internal;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Simployer.Utilities.Http.Authentication.ClientCredentials.Services
{
    /// <summary>
    /// Access token cache, that is thread-safe and supporting both sync and async retrieval of access tokens
    /// </summary>
    public class AccessTokenCache : IAccessTokenCache
    {
        private readonly Dictionary<AudienceRef, CachedAccessToken> cache = new Dictionary<AudienceRef, CachedAccessToken>();
        private readonly SemaphoreSlim semaphore = new SemaphoreSlim(1);

        public string GetAccessToken(ClientCredentialsAudienceConfiguration audience, ClientCredentialsAuthorityConfiguration authority, Func<ClientCredentialsAudienceConfiguration, ClientCredentialsAuthorityConfiguration, CancellationToken, CachedAccessToken> addFunc)
        {
            var key = new AudienceRef { Audience = audience.Audience, Authority = authority.Authority.ToString() };



            var value = cache.ContainsKey(key) ? cache[key] : null;
            if (value == null || value.Expired)
            {
                semaphore.Wait();
                try
                {
                    value = cache.ContainsKey(key) ? cache[key] : null;
                    if (value == null || value.Expired)
                    {
                        value = addFunc(audience, authority, default);
                        if (value == null)
                        {
                            throw new ClientCredentialsExchangeException($"Failed to get access token for audience '{audience.Audience}' in authority '{authority.Authority}'");
                        }
                        cache[key] = value;
                    }
                }
                finally
                {
                    semaphore.Release();
                }
            }

            return value.AccessToken;
        }

        public async Task<string> GetAccessTokenAsync(ClientCredentialsAudienceConfiguration audience, ClientCredentialsAuthorityConfiguration authority, Func<ClientCredentialsAudienceConfiguration, ClientCredentialsAuthorityConfiguration, CancellationToken, Task<CachedAccessToken>> addFunc, CancellationToken cancellationToken)
        {
            var key = new AudienceRef { Audience = audience.Audience, Authority = authority.Authority.ToString() };

            var value = cache.ContainsKey(key) ? cache[key] : null;
            if (value == null || value.Expired)
            {
                await semaphore.WaitAsync();
                try
                {
                    value = cache.ContainsKey(key) ? cache[key] : null;
                    if (value == null || value.Expired)
                    {
                        value = await addFunc(audience, authority, default);
                        if (value == null)
                        {
                            throw new ClientCredentialsExchangeException($"Failed to get access token for audience '{audience.Audience}' in authority '{authority.Authority}'");
                        }
                        cache[key] = value;
                    }
                }
                finally
                {
                    semaphore.Release();
                }
            }

            return value.AccessToken;
        }

        public void Invalidate(ClientCredentialsAudienceConfiguration audience, ClientCredentialsAuthorityConfiguration authority, string accessToken)
        {
            var key = new AudienceRef { Audience = audience.Audience, Authority = authority.Authority.ToString() };
            semaphore.Wait();
            try
            {
                var existing = cache.ContainsKey(key) ? cache[key] : null;
                if (existing != null && existing.AccessToken == accessToken)
                    cache.Remove(key);
            }
            finally { semaphore.Release(); }
        }
    }
}
