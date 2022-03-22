using Simployer.Utilities.Http.Authentication.ClientCredentials.Configuration;
using Simployer.Utilities.Http.Authentication.ClientCredentials.Internal;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Simployer.Utilities.Http.Authentication.ClientCredentials.Services
{
    public interface IAccessTokenCache
    {
        string GetAccessToken(ClientCredentialsAudienceConfiguration audience, ClientCredentialsAuthorityConfiguration authority, Func<ClientCredentialsAudienceConfiguration, ClientCredentialsAuthorityConfiguration, CancellationToken, CachedAccessToken> addFunc);
        Task<string> GetAccessTokenAsync(ClientCredentialsAudienceConfiguration audience, ClientCredentialsAuthorityConfiguration authority, Func<ClientCredentialsAudienceConfiguration, ClientCredentialsAuthorityConfiguration, CancellationToken, Task<CachedAccessToken>> addFunc, CancellationToken cancellationToken);
        void Invalidate(ClientCredentialsAudienceConfiguration audience, ClientCredentialsAuthorityConfiguration authority, string accessToken);
    }
}
