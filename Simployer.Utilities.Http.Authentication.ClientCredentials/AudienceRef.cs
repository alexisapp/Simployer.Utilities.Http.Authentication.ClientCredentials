namespace Simployer.Utilities.Http.Authentication.ClientCredentials
{
    internal struct AudienceRef
    {
        public string Audience;
        public string Authority;

        public static implicit operator AudienceRef(string s) => new AudienceRef { Audience = s };
    }
}
