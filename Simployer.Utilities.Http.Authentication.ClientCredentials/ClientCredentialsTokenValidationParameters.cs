using Microsoft.IdentityModel.Tokens;

namespace Simployer.Utilities.Http.Authentication.ClientCredentials
{
    public class ClientCredentialsTokenValidationParameters : TokenValidationParameters
    {
        public bool UseKeysFromAuthorityJwks { get; set; } = true;
    }
}
