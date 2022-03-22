using Simployer.Utilities.Http.Authentication.ClientCredentials.Configuration;
using Simployer.Utilities.Http.Authentication.ClientCredentials.Handlers;
using Simployer.Utilities.Http.Authentication.ClientCredentials.Services;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Extension methods for <see cref="IServiceCollection"/> and <see cref="IHttpClientBuilder"/> classes
    /// </summary>
    public static class HttpClientCredentialsExtensions
    {
        /// <summary>
        /// Configures a authority for client credentials authentication
        /// </summary>
        /// <param name="services">The DI container</param>
        /// <param name="name">The name of the authority.</param>
        /// <param name="configAction">The configuration action</param>
        /// <returns></returns>
        public static IServiceCollection ConfigureClientCredentialsAuthority(this IServiceCollection services, string name, Action<ClientCredentialsAuthorityConfiguration> configAction)
        {
            var config = new ClientCredentialsAuthorityConfiguration(name);
            configAction(config);
            return services
                .AddSingleton(config);
        }

        /// <summary>
        /// Adds the required services to perform client credentials authentication to the DI container
        /// </summary>
        /// <param name="services">The DI container</param>
        /// <returns><paramref name="services"/></returns>
        public static IServiceCollection AddHttpClientCredentialsAuthentication(this IServiceCollection services)
        {
            return services
                .AddSingleton<IAccessTokenCache, AccessTokenCache>()
                .AddSingleton<IAccessTokenValidationService, AccessTokenValidationService>()
                .AddSingleton<IClientCredentialsService, ClientCredentialsService>();
        }

        /// <summary>
        /// Adds a client credentials authentication handler to a <see cref="IHttpClientBuilder"/>
        /// </summary>
        /// <param name="builder">The <see cref="IHttpClientBuilder"/></param>
        /// <param name="audience">The audience the handler should authenticate against</param>
        /// <param name="authority">Optional authority. If null, uses the first authority found which has the <paramref name="audience"/></param>
        /// <returns><paramref name="builder"/></returns>
        public static IHttpClientBuilder AddClientCredentialsAuthentication(this IHttpClientBuilder builder, string audience, string authority = null)
        {
            return builder.AddHttpMessageHandler(sp => new ClientCredentialsAuthenticationHandler(audience, authority, sp.GetRequiredService<IClientCredentialsService>()));
        }
    }
}
