using System.Collections.Generic;
using System.Net.Http;
using Microsoft.Extensions.Primitives;

namespace Sinch.Verification.SinchEvents
{
    /// <summary>
    ///     Verification Sinch Events - parse incoming event payloads, validate authentication headers,
    ///     and serialize event responses.
    /// </summary>
    public interface IVerificationSinchEvents
    {
        /// <summary>
        ///     Parse a Verification Sinch Event from a raw JSON string.
        /// </summary>
        IVerificationSinchEvent ParseEvent(string json);

        /// <summary>
        ///     Validate the authentication header sent by Sinch.
        /// </summary>
        bool ValidateAuthenticationHeader(
            HttpMethod method,
            string path,
            Dictionary<string, IEnumerable<string>> headers,
            string body);

        /// <summary>
        ///     Validate the authentication header sent by Sinch.
        ///     Use this overload with ASP.NET Core <c>request.Headers</c>.
        /// </summary>
        bool ValidateAuthenticationHeader(
            HttpMethod method,
            string path,
            IEnumerable<KeyValuePair<string, StringValues>> headers,
            string body);

        /// <summary>
        ///     Serialize a verification request event response payload.
        /// </summary>
        string SerializeResponse(VerificationStartEventResponseBase response);
    }
}