using Simployer.Utilities.Http.Authentication.ClientCredentials.Configuration;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Simployer.Utilities.Http.Authentication.ClientCredentials.Services
{

    /// <summary>
    /// A service for validating access tokens
    /// </summary>
    /// <seealso cref="AccessTokenValidationService"/>
    public interface IAccessTokenValidationService
    {
#if NET6_0_OR_GREATER
        /// <summary>Validate a access token</summary>
        /// <param name="audience">The audience to validate against.</param>
        /// <param name="authority">The authority to validate against.</param>
        /// <param name="token">The JWT access token.</param>
        /// <returns>
        /// A <see cref="ClaimsPrincipal"/>
        /// </returns>
        /// <exception cref="System.Exception">If validation fails</exception>
        ClaimsPrincipal ValidateToken(ClientCredentialsAudienceConfiguration audience, ClientCredentialsAuthorityConfiguration authority, string token);
#endif

        /// <summary>Validate a access token</summary>
        /// <param name="audience">The audience to validate against.</param>
        /// <param name="authority">The authority to validate against.</param>
        /// <param name="token">The JWT access token.</param>
        /// <returns>
        /// A <see cref="ClaimsPrincipal"/>
        /// </returns>
        /// <exception cref="System.Exception">If validation fails</exception>
        Task<ClaimsPrincipal> ValidateTokenAsync(ClientCredentialsAudienceConfiguration audience, ClientCredentialsAuthorityConfiguration authority, string token);
    }
}
