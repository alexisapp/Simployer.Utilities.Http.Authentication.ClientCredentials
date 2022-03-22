using System;
using System.Runtime.Serialization;

namespace Simployer.Utilities.Http.Authentication.ClientCredentials
{
    public class ClientCredentialsExchangeException : Exception
    {
        public ClientCredentialsExchangeException()
        {
        }

        public ClientCredentialsExchangeException(string message) : base(message)
        {
        }

        public ClientCredentialsExchangeException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ClientCredentialsExchangeException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
