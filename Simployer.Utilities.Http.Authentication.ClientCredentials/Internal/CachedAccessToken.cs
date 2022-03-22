using System;

namespace Simployer.Utilities.Http.Authentication.ClientCredentials.Internal
{
    /// <summary>
    /// A cached access token
    /// </summary>
    public class CachedAccessToken
    {
        /// <summary>
        /// Gets or sets the JWT access token.
        /// </summary>
        /// <value>
        /// The JWT access token.
        /// </value>
        public string AccessToken { get; set; }

        /// <summary>
        /// Gets or sets the point in time when the access token expires
        /// </summary>
        /// <value>
        /// The expiration date of the access token
        /// </value>
        public DateTime Expires { get; set; }

        /// <summary>
        /// Gets a value indicating whether this <see cref="CachedAccessToken"/> is expired.
        /// </summary>
        /// <value>
        ///   <c>true</c> if expired; otherwise, <c>false</c>.
        /// </value>
        public bool Expired => DateTime.Now > Expires;
    }
}
