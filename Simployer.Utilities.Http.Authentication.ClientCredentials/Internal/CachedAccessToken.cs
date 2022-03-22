using System;

namespace Simployer.Utilities.Http.Authentication.ClientCredentials.Internal
{
    public class CachedAccessToken
    {
        public string AccessToken { get; set; }
        public DateTime Expires { get; set; }

        public bool Expired => DateTime.Now > Expires;
    }
}
