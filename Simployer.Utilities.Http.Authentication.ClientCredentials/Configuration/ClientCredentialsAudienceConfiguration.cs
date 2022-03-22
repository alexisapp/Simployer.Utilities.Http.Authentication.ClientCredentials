namespace Simployer.Utilities.Http.Authentication.ClientCredentials.Configuration
{
    public class ClientCredentialsAudienceConfiguration
    {
        public string Audience { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public ClientCredentialsTokenValidationParameters TokenValidationParameters { get; set; }
    }
}
