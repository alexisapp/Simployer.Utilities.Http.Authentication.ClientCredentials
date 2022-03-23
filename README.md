Simployer.Utilities.Http.Authentication.ClientCredentials
==============================================================

[![build](https://github.com/simployer/Simployer.Utilities.Http.Authentication.ClientCredentials/actions/workflows/build-deploy.yaml/badge.svg)](https://github.com/simployer/Simployer.Utilities.Http.Authentication.ClientCredentials/actions/workflows/build-deploy.yaml)

[View Documentation](https://simployer.github.io/Simployer.Utilities.Http.Authentication.ClientCredentials)

[View API Reference](https://simployer.github.io/Simployer.Utilities.Http.Authentication.ClientCredentials/api)

Quickstart
---------------------
```csharp
// Add required services to DI container
services.AddHttpClientCredentialsAuthentication();
 
// Add authority and audiences
services.ConfigureClientCredentialsAuthority("Auth0", config => {
    config.Authority = new Uri(Configuration["Auth0:Authority"]);
    config.TokenPath = "oauth/token";
    config.JwksPath = ".well-known/jwks.json";
    config.Audiences.Add(new ClientCredentialsAudienceConfiguration {
        Audience = "https://apis.simployer.com";
        ClientId = Configuration["Auth0:ClientId"];
        ClientSecret = Configuration["Auth0:ClientSecret"];
    });
});

// Configure a named HttpClient called 'Simployer API Services' that require client credentials authentication against the `https://apis.simployer.com` audience.
// Also add a typed client `MyApiService` that uses this named HttpClient
services.AddHttpClient("Simployer API Services")
    .AddClientCredentialsAuthentication("https://apis.simployer.com")
    .AddTypedClient<MyApiService>();
```
