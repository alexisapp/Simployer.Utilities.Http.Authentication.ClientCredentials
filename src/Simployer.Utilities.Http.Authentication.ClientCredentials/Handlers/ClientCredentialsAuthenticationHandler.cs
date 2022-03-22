using Simployer.Utilities.Http.Authentication.ClientCredentials.Services;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace Simployer.Utilities.Http.Authentication.ClientCredentials.Handlers
{
    /// <summary>
    /// A HTTP client handler that performs client credentials authentication
    /// </summary>
    /// <seealso cref="System.Net.Http.DelegatingHandler" />
    public class ClientCredentialsAuthenticationHandler : DelegatingHandler
    {
        private readonly IClientCredentialsService clientCredentialsService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientCredentialsAuthenticationHandler"/> class.
        /// </summary>
        /// <param name="audience">The audience this handler is targeting.</param>
        /// <param name="authority">The authority this handler uses. If null, uses the first authority found that supports <paramref name="audience"/></param>
        /// <param name="clientCredentialsService">A <see cref="IClientCredentialsService"/> instance used for authentication</param>
        /// <exception cref="System.ArgumentException">'<paramref name="audience"/>' cannot be null or whitespace</exception>
        /// <exception cref="System.ArgumentNullException">clientCredentialsService</exception>
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

        /// <summary>
        /// Gets the audience used for authentication.
        /// </summary>
        /// <value>
        /// The audience.
        /// </value>
        public string Audience { get; }

        /// <summary>
        /// Gets the name of the authority used for authentication.
        /// </summary>
        /// <value>
        /// The authority name. If null, uses the first authority found that supports <see cref="Audience"/>
        /// </value>
        public string Authority { get; }

        private void OnResponse(HttpResponseMessage response, string accessToken)
        {
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                clientCredentialsService.InvalidateAccessToken(Audience, Authority, accessToken);
            }
        }

#if NET6_0_OR_GREATER
        /// <inheritdoc/>
        protected override HttpResponseMessage Send(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            string accessToken = clientCredentialsService.AuthenticateForAudience(Audience, Authority);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var response = base.Send(request, cancellationToken);
            OnResponse(response, accessToken);
            return response;
        }
#endif

        /// <inheritdoc/>
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
