using Simployer.Utilities.Http.Authentication.ClientCredentials.Configuration;
using Simployer.Utilities.Http.Authentication.ClientCredentials.Internal;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Simployer.Utilities.Http.Authentication.ClientCredentials.Services
{
    public class ClientCredentialsService : IClientCredentialsService
    {
        private readonly IAccessTokenCache accessTokenCache;
        private readonly IAccessTokenValidationService accessTokenValidationService;
        private readonly HttpClient httpClient;
        private readonly ClientCredentialsAuthorityConfiguration[] configurations;

        public ClientCredentialsService(IAccessTokenCache accessTokenCache, IAccessTokenValidationService accessTokenValidationService, HttpClient httpClient, IEnumerable<ClientCredentialsAuthorityConfiguration> configurations)
        {
            this.accessTokenCache = accessTokenCache ?? throw new ArgumentNullException(nameof(accessTokenCache));
            this.accessTokenValidationService = accessTokenValidationService ?? throw new ArgumentNullException(nameof(accessTokenValidationService));
            this.httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            this.configurations = (configurations ?? throw new ArgumentNullException(nameof(configurations))).ToArray();
        }

        private HttpRequestMessage CreateRequest(ClientCredentialsAudienceConfiguration audience, ClientCredentialsAuthorityConfiguration authority)
        {
            var uri = new Uri(authority.Authority, authority.TokenPath);
            var request = new HttpRequestMessage(HttpMethod.Post, uri);
            var requestJson = JsonSerializer.Serialize(new
            {
                grant_type = "client_credentials",
                audience = audience.Audience,
                client_id = audience.ClientId ?? authority.ClientId ?? throw new ClientCredentialsExchangeException($"Audience '{audience.Audience}' for authority '{authority.Authority}' has no Client ID"),
                client_secret = audience.ClientSecret ?? authority.ClientSecret ?? throw new ClientCredentialsExchangeException($"Audience '{audience.Audience}' for authority '{authority.Authority}' has no Client Secret"),
            });
            request.Content = new StringContent(requestJson, Encoding.UTF8, "application/json");
            return request;
        }

        private CachedAccessToken ProcessResponse(HttpResponseMessage response, string responseString)
        {
            if(response.IsSuccessStatusCode)
            {
                var tokenResponse = JsonSerializer.Deserialize<AccessTokenResponse>(responseString);
                if(tokenResponse == null || tokenResponse.AccessToken == null || !tokenResponse.TokenType.Equals("bearer", StringComparison.InvariantCultureIgnoreCase))
                {
                    throw new ClientCredentialsExchangeException("Invalid access token response");
                }
                var expires = DateTime.Now.AddSeconds(tokenResponse.ExpiresIn);

                return new CachedAccessToken { AccessToken = tokenResponse.AccessToken, Expires = expires };
            }

            throw new ClientCredentialsExchangeException("Client credentials exchange failed");
        }

        private async Task<CachedAccessToken> FetchAccessTokenAsync(ClientCredentialsAudienceConfiguration audience, ClientCredentialsAuthorityConfiguration authority, CancellationToken cancellationToken)
        {
            var request = CreateRequest(audience, authority);
            var response = await httpClient.SendAsync(request, cancellationToken);
            var responseString = await response.Content.ReadAsStringAsync();
            var accessToken = ProcessResponse(response, responseString);
            await accessTokenValidationService.ValidateTokenAsync(audience, authority, accessToken.AccessToken);
            return accessToken;
        }

#if NET6_0_OR_GREATER
        private CachedAccessToken FetchAccessToken(ClientCredentialsAudienceConfiguration audience, ClientCredentialsAuthorityConfiguration authority, CancellationToken cancellationToken)
        {
            var request = CreateRequest(audience, authority);
            var response = httpClient.Send(request, cancellationToken);

            using (var contentStream = response.Content.ReadAsStream())
            using (var textReader = new StreamReader(contentStream))
            {
                var accessToken = ProcessResponse(response, textReader.ReadToEnd());
                accessTokenValidationService.ValidateTokenAsync(audience, authority, accessToken.AccessToken);
                return accessToken;
            }
        }
#endif

        private (ClientCredentialsAuthorityConfiguration, ClientCredentialsAudienceConfiguration) GetConfig(string audience, string authority)
        {
            var authorityConfig = !string.IsNullOrWhiteSpace(authority)
                ? configurations.FirstOrDefault(x => x.Authority == new Uri(authority))
                : configurations?.FirstOrDefault(x => x.Audiences.Any(y => y.Audience == audience));
            var audienceConfig = authorityConfig?.Audiences.FirstOrDefault(x => x.Audience == audience);

            if (authorityConfig == null)
            {
                throw new ClientCredentialsExchangeException();
            }

            if (audienceConfig == null)
            {
                throw new ClientCredentialsExchangeException();
            }

            return (authorityConfig, audienceConfig);
        }

#if NET6_0_OR_GREATER
        public string AuthenticateForAudience(string audience, string authority)
        {
            var (authorityConfig, audienceConfig) = GetConfig(audience, authority);
            return accessTokenCache.GetAccessToken(audienceConfig, authorityConfig, FetchAccessToken) ?? throw new ClientCredentialsExchangeException();
        }
#endif

        public async Task<string> AuthenticateForAudienceAsync(string audience, string authority, CancellationToken cancellationToken)
        {
            var (authorityConfig, audienceConfig) = GetConfig(audience, authority);

            return await accessTokenCache.GetAccessTokenAsync(audienceConfig, authorityConfig, FetchAccessTokenAsync, cancellationToken) ?? throw new ClientCredentialsExchangeException();
        }

        public void InvalidateAccessToken(string audience, string authority, string accessToken)
        {
            var (authorityConfig, audienceConfig) = GetConfig(audience, authority);
            accessTokenCache.Invalidate(audienceConfig, authorityConfig, accessToken);
        }
    }
}
