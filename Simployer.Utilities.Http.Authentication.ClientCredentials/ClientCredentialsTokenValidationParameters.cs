using Microsoft.IdentityModel.Tokens;

namespace Simployer.Utilities.Http.Authentication.ClientCredentials
{
    /// <summary>
    /// Extended TokenValidationParameters class that supports a UseKeyFromAuthorityJwks property for determining if a Jwks document should be downloaded from the authority for verifying signatures
    /// </summary>
    /// <seealso cref="Microsoft.IdentityModel.Tokens.TokenValidationParameters" />
    public class ClientCredentialsTokenValidationParameters : TokenValidationParameters
    {
        /// <summary>
        /// Gets or sets a value indicating whether to use keys from authority JWKS endpoint
        /// </summary>
        /// <value>
        ///   <c>true</c> if using keys from authority JWKS; otherwise, <c>false</c>.
        /// </value>
        public bool UseKeysFromAuthorityJwks { get; set; } = true;
    }
}
