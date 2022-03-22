using Simployer.Utilities.Http.Authentication.ClientCredentials.Services;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace Simployer.Utilities.Http.Authentication.ClientCredentials.Handlers
{
    public class ClientCredentialsAuthenticationHandler : DelegatingHandler
    {
        private readonly IClientCredentialsService clientCredentialsService;

        public ClientCredentialsAuthenticationHandler(string audience, string authority, IClientCredentialsService clientCredentialsService)
        {
            if (string.IsNullOrWhiteSpace(audience))
            {
                throw new ArgumentException($"'{nameof(audience)}' cannot be null or whitespace.", nameof(audience));
            }

            Audience = audience;
            Authority = authority;
            this.clientCredentialsService = clientCredentialsService ?? throw new ArgumentNullException(nameof(clientCredentialsService));
        }

        public string Audience { get; }
        public string Authority { get; }

        protected void OnResponse(HttpResponseMessage response, string accessToken)
        {
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                clientCredentialsService.InvalidateAccessToken(Audience, Authority, accessToken);
            }
        }

#if NET6_0_OR_GREATER
        protected override HttpResponseMessage Send(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            string accessToken = clientCredentialsService.AuthenticateForAudience(Audience, Authority);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var response = base.Send(request, cancellationToken);
            OnResponse(response, accessToken);
            return response;
        }
#endif

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            string accessToken = await clientCredentialsService.AuthenticateForAudienceAsync(Audience, Authority,  cancellationToken);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var response = await base.SendAsync(request, cancellationToken);
            OnResponse(response, accessToken);
            return response;
        }
    }
}
