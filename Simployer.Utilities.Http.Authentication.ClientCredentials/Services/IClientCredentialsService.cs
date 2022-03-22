using System.Threading;
using System.Threading.Tasks;

namespace Simployer.Utilities.Http.Authentication.ClientCredentials.Services
{
    public interface IClientCredentialsService
    {
        void InvalidateAccessToken(string audience, string authority, string accessToken);
#if NET6_0_OR_GREATER
        string AuthenticateForAudience(string audience, string authority);
#endif
        Task<string> AuthenticateForAudienceAsync(string audience, string authority, CancellationToken cancellation);
    }
}
