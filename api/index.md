# Components

The [](xref:System.Net.Http.HttpClient?displayProperty=fullName) handler that is responsible for client credentials authentication is
[](xref:Simployer.Utilities.Http.Authentication.ClientCredentials.Handlers.ClientCredentialsAuthenticationHandler).
The handler uses a [](xref:Simployer.Utilities.Http.Authentication.ClientCredentials.Services.IAccessTokenCache) service to cache
access tokens. If no token is found in the cache, the service performs a client credentials request to a OAuth2 server.
The access token received from the OAuth2 server is validated using a instance of [](xref:Simployer.Utilities.Http.Authentication.ClientCredentials.Services.IAccessTokenValidationService)
