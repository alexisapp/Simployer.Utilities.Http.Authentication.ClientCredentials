namespace Simployer.Utilities.Http.Authentication.ClientCredentials.Configuration
{
    /// <summary>
    /// Configuration class for a audience within a authority
    /// </summary>
    public class ClientCredentialsAudienceConfiguration
    {
        /// <summary>
        /// Gets or sets the audience name.
        /// </summary>
        /// <value>
        /// The audience that this configuration applies for.
        /// </value>
        public string Audience { get; set; }

        /// <summary>
        /// Gets or sets the Client Id for this Audience.
        /// </summary>
        /// <value>
        /// The client id to use for this audience. If null, the client id specificed in the authority is used instead
        /// </value>
        public string ClientId { get; set; }

        /// <summary>
        /// Gets or sets the Client Secret for this Audience.
        /// </summary>
        /// <value>
        /// The client secret. If null, the client secret from the authority is used instead
        /// </value>
        public string ClientSecret { get; set; }

        /// <summary>
        /// Gets or sets the token validation parameters. If null, uses default TokenValidationParameters with issuer validation, audience validation and signature validation
        /// </summary>
        /// <value>
        /// The token validation parameters.
        /// </value>
        public ClientCredentialsTokenValidationParameters TokenValidationParameters { get; set; }
    }
}
