# Getting Started

## Dependencies
Install the required package to the project:
`dotnet add package Simployer.Utilities.Http.Authentication.ClientCredentials`

## Setup

For the package to work, the required services must first be added to the 
DI container

```csharp
// Add required services to DI container
services.AddHttpClientCredentialsAuthentication();
```

Atleast one OAuth2 authority must be added. In this case we use "Auth0" for its descriptive name,
assuming we are going to use Auth0 for our OAuth2 server. The Authority Uri is fetched from Configuration
using the key `Auth0:Authority`. The authority must have a `TokenPath` specified (unless it is `oauth/token`),
and it should also have `JwksPath` specified. The `TokenPath` is a path to the OAuth2 token endpoint,
relative to the Authority Uri. The `JwksPath` is a path to a `Json Web Key Set` document, again,
relative to the Authority Uri.

A audience must also be provided. In most cases an audience is a protected API.

The audience must also have a ClientId + ClientSecret

```csharp 
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
```

Adding client credentials authentication works in a similar fashion to how other handlers are added
to HttpClient instances via `Microsoft.Extensions.Http` (eg, `Polly`).
In this example, a named HttpClient with name `Simployer API Services` is added, which uses
client credentials authentication against a `https://apis.simployer.com` audience.
`MyApiService` is added as a typed client. This class will receive a HttpClient in its constructor that
uses client credentials authentication against the `https://apis.simployer.com` audience.

```csharp 
// Configure a named HttpClient called 'Simployer API Services' that require client credentials authentication against the `https://apis.simployer.com` audience.
// Also add a typed client `MyApiService` that uses this named HttpClient
services.AddHttpClient("Simployer API Services")
    .AddClientCredentialsAuthentication("https://apis.simployer.com")
    .AddTypedClient<MyApiService>();
```
