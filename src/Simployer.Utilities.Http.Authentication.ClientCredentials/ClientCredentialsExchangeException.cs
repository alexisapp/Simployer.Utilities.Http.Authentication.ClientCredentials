using System;
using System.Runtime.Serialization;

namespace Simployer.Utilities.Http.Authentication.ClientCredentials
{
    /// <summary>
    /// Exception thrown on error during a client credentials exchange
    /// </summary>
    public class ClientCredentialsExchangeException : Exception
    {
        /// <summary>Initializes a new instance of the <see cref="ClientCredentialsExchangeException" /> class.</summary>
        public ClientCredentialsExchangeException()
        {
        }

        /// <summary>Initializes a new instance of the <see cref="ClientCredentialsExchangeException" /> class.</summary>
        /// <param name="message">The message that describes the error.</param>
        public ClientCredentialsExchangeException(string message) : base(message)
        {
        }

        /// <summary>Initializes a new instance of the <see cref="ClientCredentialsExchangeException" /> class.</summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
        public ClientCredentialsExchangeException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>Initializes a new instance of the <see cref="ClientCredentialsExchangeException" /> class.</summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo">SerializationInfo</see> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext">StreamingContext</see> that contains contextual information about the source or destination.</param>
        protected ClientCredentialsExchangeException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
