using Microsoft.IdentityModel.Tokens;
using Simployer.Utilities.Http.Authentication.ClientCredentials.Configuration;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Simployer.Utilities.Http.Authentication.ClientCredentials.Services
{
    public class AccessTokenValidationService : IAccessTokenValidationService
    {
        private readonly HttpClient httpClient;

        public AccessTokenValidationService(HttpClient httpClient)
        {
            this.httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        private Uri GetJwksUri(ClientCredentialsAuthorityConfiguration authority)
        {
            return new Uri(authority.Authority, ".well-known/jwks.json");
        }

#if NET6_0_OR_GREATER
        private JsonWebKeySet GetJwks(ClientCredentialsAuthorityConfiguration authority, ClientCredentialsAudienceConfiguration audience)
        {
            var jwksUri = GetJwksUri(authority);
            var request = new HttpRequestMessage(HttpMethod.Get, jwksUri);
            var response = httpClient.Send(request);

            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    break;
                default:
                    throw new ClientCredentialsExchangeException($"Failed to get Json Web Key Set for authority '{authority.Authority}'");
            }

            using (var responseStream = response.Content.ReadAsStream())
            using (var reader = new StreamReader(responseStream))
            {
                var responseText = reader.ReadToEnd();
                var jwks = JsonWebKeySet.Create(responseText);
                return jwks;
            }
        }
#endif

        private async Task<JsonWebKeySet> GetJwksAsync(ClientCredentialsAuthorityConfiguration authority, ClientCredentialsAudienceConfiguration audience)
        {
            var jwksUri = GetJwksUri(authority);
            var request = new HttpRequestMessage(HttpMethod.Get, jwksUri);
            var response = await httpClient.SendAsync(request);

            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    break;
                default:
                    throw new ClientCredentialsExchangeException($"Failed to get Json Web Key Set for authority '{authority.Authority}'");
            }

            var responseText = await response.Content.ReadAsStringAsync();
            var jwks = JsonWebKeySet.Create(responseText);
            return jwks;
        }

        private ClaimsPrincipal ValidateWithJwks(ClientCredentialsAudienceConfiguration audience, ClientCredentialsAuthorityConfiguration authority, string accessToken, JsonWebKeySet jwks)
        {
            var tokenValidationParametersOriginal = audience.TokenValidationParameters ?? new ClientCredentialsTokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
            };
            var useAuthorityJwks = tokenValidationParametersOriginal.UseKeysFromAuthorityJwks;
            var tokenValidationParameters = tokenValidationParametersOriginal.Clone();

            IEnumerable<SecurityKey> GetIssuerSigningKey(string token, SecurityToken securityToken, string kid, TokenValidationParameters validationParameters)
            {
                var list = new List<SecurityKey>();

                if(jwks != null)
                {
                    list.AddRange(jwks.GetSigningKeys());
                }

                if (validationParameters.IssuerSigningKey != null)
                {
                    list.Add(validationParameters.IssuerSigningKey);
                }

                if (validationParameters.IssuerSigningKeys != null)
                {
                    list.AddRange(validationParameters.IssuerSigningKeys);
                }

                return list.Where(x => x.KeyId == kid).Distinct().ToArray();
            }

            if (tokenValidationParameters.IssuerSigningKeyResolver == null)
            {
                tokenValidationParameters.IssuerSigningKeyResolver = GetIssuerSigningKey;
            }

            if (tokenValidationParameters.ValidIssuer == null && tokenValidationParameters.ValidIssuers.IsNullOrEmpty())
            {
                tokenValidationParameters.ValidIssuer = authority.Authority.ToString();
            }

            if (tokenValidationParameters.ValidAudience == null && tokenValidationParameters.ValidAudiences.IsNullOrEmpty())
            {
                tokenValidationParameters.ValidAudience = audience.Audience;
            }

            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            return handler.ValidateToken(accessToken, tokenValidationParameters, out var validatedToken);
        }

        public ClaimsPrincipal ValidateToken(ClientCredentialsAudienceConfiguration audience, ClientCredentialsAuthorityConfiguration authority, string token)
        {
#if NET6_0_OR_GREATER
            var useJwks = audience.TokenValidationParameters?.UseKeysFromAuthorityJwks == true || audience.TokenValidationParameters == null;

            return ValidateWithJwks(audience, authority, token, useJwks ? GetJwks(authority, audience) : null);
#else
            return ValidateTokenAsync(audience, authority, token).ConfigureAwait(false).GetAwaiter().GetResult();
#endif
        }

        public async Task<ClaimsPrincipal> ValidateTokenAsync(ClientCredentialsAudienceConfiguration audience, ClientCredentialsAuthorityConfiguration authority, string token)
        {
            var useJwks = audience.TokenValidationParameters?.UseKeysFromAuthorityJwks == true || audience.TokenValidationParameters == null;

            return ValidateWithJwks(audience, authority, token, useJwks ? await GetJwksAsync(authority, audience) : null);
        }
    }
}
