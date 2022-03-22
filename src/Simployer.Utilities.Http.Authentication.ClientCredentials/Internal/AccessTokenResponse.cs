using System.Text.Json.Serialization;

namespace Simployer.Utilities.Http.Authentication.ClientCredentials.Internal
{
    /// <summary>
    /// A OAuth2 access token response
    /// </summary>
    public class AccessTokenResponse
    {
        /// <summary>
        /// Gets or sets the access token.
        /// </summary>
        /// <value>
        /// The access token.
        /// </value>
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; }

        /// <summary>
        /// Gets or sets the type of the token.
        /// </summary>
        /// <value>
        /// The type of the token. Should be 'Bearer'
        /// </value>
        [JsonPropertyName("token_type")]
        public string TokenType { get; set; }

        /// <summary>
        /// Gets or sets the number of seconds until the access token expires
        /// </summary>
        /// <value>
        /// The lifetime of the access token in seconds
        /// </value>
        [JsonPropertyName("expires_in")]
        public long ExpiresIn { get; set; }
    }
}
