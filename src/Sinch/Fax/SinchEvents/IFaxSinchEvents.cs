using System.Collections.Generic;
using Microsoft.Extensions.Primitives;

namespace Sinch.Fax.SinchEvents
{
    /// <summary>
    ///     Fax Sinch Events — parse incoming event payloads and validate
    ///     HMAC signatures sent by Sinch to your event destination.
    /// </summary>
    public interface IFaxSinchEvents
    {
        /// <summary>Parse a Fax Sinch Event from a raw JSON string.</summary>
        /// <param name="json">Raw JSON string of the incoming Fax Sinch Event payload.</param>
        IFaxSinchEvent ParseEvent(string json);

        /// <summary>
        ///     Validate the HMAC authentication header sent by Sinch.
        /// </summary>
        /// <param name="hmacSecret">Your HMAC secret.</param>
        /// <param name="headers">All HTTP headers from the incoming request.</param>
        /// <param name="body">Raw request body string.</param>
        /// <returns><c>true</c> if the signature is valid.</returns>
        bool ValidateAuthenticationHeader(
            string hmacSecret,
            IEnumerable<KeyValuePair<string, IEnumerable<string>>> headers,
            string body);

        /// <summary>
        ///     Validate the HMAC authentication header sent by Sinch.
        ///     Use this overload with ASP.NET Core <c>request.Headers</c>.
        /// </summary>
        /// <param name="hmacSecret">Your HMAC secret.</param>
        /// <param name="headers">All HTTP headers from the incoming request.</param>
        /// <param name="body">Raw request body string.</param>
        /// <returns><c>true</c> if the signature is valid.</returns>
        bool ValidateAuthenticationHeader(
            string hmacSecret,
            IEnumerable<KeyValuePair<string, StringValues>> headers,
            string body);
    }
}
