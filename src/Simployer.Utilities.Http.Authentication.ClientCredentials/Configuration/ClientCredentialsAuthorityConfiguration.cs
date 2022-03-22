using System;
using System.Collections.Generic;

namespace Simployer.Utilities.Http.Authentication.ClientCredentials.Configuration
{

    /// <summary>
    /// A configuration class for client credentials authorities
    /// </summary>
    public class ClientCredentialsAuthorityConfiguration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ClientCredentialsAuthorityConfiguration"/> class.
        /// </summary>
        /// <param name="name">The name of the authority</param>
        /// <exception cref="System.ArgumentException"><paramref name="name"/> cannot be null or whitespace.</exception>
        public ClientCredentialsAuthorityConfiguration(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException($"'{nameof(name)}' cannot be null or whitespace.", nameof(name));
            }

            Name = name;
        }

        /// <summary>
        /// Gets the name of the authority.
        /// </summary>
        /// <value>
        /// The authority name
        /// </value>
        public string Name { get; }

        /// <summary>
        /// Gets or sets the authority URI
        /// </summary>
        /// <value>
        /// The authority URI
        /// </value>
        public Uri Authority { get; set; }

        /// <summary>
        /// Gets or sets the token path.
        /// </summary>
        /// <value>
        /// The token path. The default value is 'oauth/token'
        /// </value>
        public string TokenPath { get; set; } = "oauth/token";

        /// <summary>
        /// Gets or sets the JWKS path.
        /// </summary>
        /// <value>
        /// The JWKS path. The default is .well-known/jwks.json
        /// </value>
        public string JwksPath { get; set; } = ".well-known/jwks.json";

        /// <summary>
        /// Gets or sets the default Client ID for this authority. This is used if no audience specific Client ID is specified
        /// </summary>
        /// <value>
        /// The client id
        /// </value>
        public string ClientId { get; set; }

        /// <summary>
        /// Gets or sets the Client Secret for this authority. This is used if no audience specific Client Secret is specified
        /// </summary>
        /// <value>
        /// The client secret.
        /// </value>
        public string ClientSecret { get; set; }

        /// <summary>
        /// Gets the audience list of this authority.
        /// </summary>
        /// <value>
        /// The audiences this authority supports
        /// </value>
        public IList<ClientCredentialsAudienceConfiguration> Audiences { get; } = new List<ClientCredentialsAudienceConfiguration>();
    }
}
