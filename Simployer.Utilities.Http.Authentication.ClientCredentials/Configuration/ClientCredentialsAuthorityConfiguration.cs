using System;
using System.Collections.Generic;

namespace Simployer.Utilities.Http.Authentication.ClientCredentials.Configuration
{
    public class ClientCredentialsAuthorityConfiguration
    {
        public ClientCredentialsAuthorityConfiguration(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException($"'{nameof(name)}' cannot be null or whitespace.", nameof(name));
            }

            Name = name;
        }

        public string Name { get; }
        public Uri Authority { get; set; }
        public string TokenPath { get; set; } = "oauth/token";
        public string JwksPath { get; set; } = ".well-known/jwks.json";
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public IList<ClientCredentialsAudienceConfiguration> Audiences { get; } = new List<ClientCredentialsAudienceConfiguration>();
    }
}
