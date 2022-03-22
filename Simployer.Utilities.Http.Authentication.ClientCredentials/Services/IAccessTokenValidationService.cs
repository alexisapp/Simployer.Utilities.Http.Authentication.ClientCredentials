using Simployer.Utilities.Http.Authentication.ClientCredentials.Configuration;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Simployer.Utilities.Http.Authentication.ClientCredentials.Services
{
    public interface IAccessTokenValidationService
    {
        ClaimsPrincipal ValidateToken(ClientCredentialsAudienceConfiguration audience, ClientCredentialsAuthorityConfiguration authority, string token);
        Task<ClaimsPrincipal> ValidateTokenAsync(ClientCredentialsAudienceConfiguration audience, ClientCredentialsAuthorityConfiguration authority, string token);
    }
}
