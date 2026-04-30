using System.Collections.Generic;
using System.Text.Json;

namespace Sinch.Numbers.SinchEvents
{
    /// <summary>
    ///     Numbers Sinch Events — parse incoming event payloads and validate
    ///     HMAC signatures sent by Sinch to your event destination.
    /// </summary>
    public interface INumbersSinchEvents
    {
        internal JsonSerializerOptions JsonSerializerOptions { get; }

        /// <summary>Parse a Numbers Sinch Event from a raw JSON string.</summary>
        INumbersSinchEvent ParseEvent(string json);

        /// <summary>
        ///     Validate the HMAC authentication header sent by Sinch.
        /// </summary>
        /// <param name="hmacSecret">Your HMAC secret.</param>
        /// <param name="headers">All HTTP headers from the incoming request.</param>
        /// <param name="body">Raw request body string.</param>
        /// <returns><c>true</c> if the signature is valid.</returns>
        bool ValidateAuthenticationHeader(
            string hmacSecret,
            IDictionary<string, IEnumerable<string>> headers,
            string body);
    }
}
