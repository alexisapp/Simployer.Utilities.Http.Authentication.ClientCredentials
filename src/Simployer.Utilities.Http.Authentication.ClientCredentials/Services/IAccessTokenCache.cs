using Simployer.Utilities.Http.Authentication.ClientCredentials.Configuration;
using Simployer.Utilities.Http.Authentication.ClientCredentials.Internal;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Simployer.Utilities.Http.Authentication.ClientCredentials.Services
{
    /// <summary>
    /// Access token cache, that is thread-safe and supporting both sync and async retrieval of access tokens
    /// </summary>
    /// <seealso cref="AccessTokenCache"/>
    public interface IAccessTokenCache
    {
        /// <summary>
        /// Get or add access token.
        /// </summary>
        /// <param name="audience">The desired access token audience.</param>
        /// <param name="authority">The access token authority.</param>
        /// <param name="addFunc">The function to call to retrieve a access token if no valid access token exists.</param>
        /// <returns>A JWT access token</returns>
        string GetOrAddAccessToken(ClientCredentialsAudienceConfiguration audience, ClientCredentialsAuthorityConfiguration authority, Func<ClientCredentialsAudienceConfiguration, ClientCredentialsAuthorityConfiguration, CancellationToken, CachedAccessToken> addFunc);

        /// <summary>
        /// Get or add access token.
        /// </summary>
        /// <param name="audience">The desired access token audience.</param>
        /// <param name="authority">The access token authority.</param>
        /// <param name="addFunc">The function to call to retrieve a access token if no valid access token exists.</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns>A JWT access token</returns>
        Task<string> GetOrAddAccessTokenAsync(ClientCredentialsAudienceConfiguration audience, ClientCredentialsAuthorityConfiguration authority, Func<ClientCredentialsAudienceConfiguration, ClientCredentialsAuthorityConfiguration, CancellationToken, Task<CachedAccessToken>> addFunc, CancellationToken cancellationToken);
        
        /// <summary>
        /// Invalidate a access token.
        /// </summary>
        /// <param name="audience">The audience the token belongs to.</param>
        /// <param name="authority">The authority the token belongs to.</param>
        /// <param name="accessToken">The JWT access token.</param>
        void Invalidate(ClientCredentialsAudienceConfiguration audience, ClientCredentialsAuthorityConfiguration authority, string accessToken);
    }
}
