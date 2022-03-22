using Simployer.Utilities.Http.Authentication.ClientCredentials.Configuration;
using Simployer.Utilities.Http.Authentication.ClientCredentials.Handlers;
using Simployer.Utilities.Http.Authentication.ClientCredentials.Services;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class HttpClientCredentialsExtensions
    {
        public static IServiceCollection ConfigureClientCredentialsAuthority(this IServiceCollection services, string name, Action<ClientCredentialsAuthorityConfiguration> configAction)
        {
            var config = new ClientCredentialsAuthorityConfiguration(name);
            configAction(config);
            return services
                .AddSingleton(config);
        }

        public static IServiceCollection AddHttpClientCredentialsAuthentication(this IServiceCollection services)
        {
            return services
                .AddSingleton<IAccessTokenCache, AccessTokenCache>()
                .AddSingleton<IAccessTokenValidationService, AccessTokenValidationService>()
                .AddSingleton<IClientCredentialsService, ClientCredentialsService>();
        }

        public static IHttpClientBuilder AddClientCredentialsAuthentication(this IHttpClientBuilder builder, string audience, string authority = null)
        {
            return builder.AddHttpMessageHandler(sp => new ClientCredentialsAuthenticationHandler(audience, authority, sp.GetRequiredService<IClientCredentialsService>()));
        }
    }
}
