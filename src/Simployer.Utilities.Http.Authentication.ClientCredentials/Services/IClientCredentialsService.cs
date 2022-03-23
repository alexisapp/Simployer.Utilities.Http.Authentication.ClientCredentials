using System.Threading;
using System.Threading.Tasks;

namespace Simployer.Utilities.Http.Authentication.ClientCredentials.Services
{
    /// <summary>
    /// A service for performing client credentials authentication
    /// </summary>
    /// <seealso cref="ClientCredentialsService"/>
    public interface IClientCredentialsService
    {
        /// <summary>
        /// Invalidates a access token. Called when a access token is rejected
        /// </summary>
        /// <param name="audience">The audience.</param>
        /// <param name="authority">The authority.</param>
        /// <param name="accessToken">The access token.</param>
        void InvalidateAccessToken(string audience, string authority, string accessToken);

#if NET6_0_OR_GREATER
        /// <summary>
        /// Authenticate against a audience.
        /// </summary>
        /// <param name="audience">The audience to authenticate against.</param>
        /// <param name="authority">The authority. If null, uses the first authority that supports <paramref name="audience"/></param>
        /// <returns>A JWT access token</returns>
        string AuthenticateForAudience(string audience, string authority);
#endif
        /// <summary>
        /// Authenticate against a audience.
        /// </summary>
        /// <param name="audience">The audience to authenticate against.</param>
        /// <param name="authority">The authority. If null, uses the first authority that supports <paramref name="audience"/></param>
        /// <param name="cancellation">The cancellation token</param>
        /// <returns>A JWT access token</returns>
        Task<string> AuthenticateForAudienceAsync(string audience, string authority, CancellationToken cancellation);
    }
}
