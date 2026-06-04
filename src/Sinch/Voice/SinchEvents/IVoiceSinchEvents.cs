using System.Collections.Generic;
using System.Net.Http;
using Microsoft.Extensions.Primitives;

namespace Sinch.Voice.SinchEvents
{
    /// <summary>
    ///     Voice Sinch Events — parse incoming event payloads, validate Sinch request signatures,
    ///     and serialize event responses.
    /// </summary>
    public interface IVoiceSinchEvents
    {
        /// <summary>
        ///     Parse a Voice Sinch Event from a raw JSON string.
        /// </summary>
        /// <param name="json">Raw JSON string of the incoming event payload.</param>
        /// <returns>The parsed <see cref="IVoiceSinchEvent" />.</returns>
        IVoiceSinchEvent ParseEvent(string json);

        /// <summary>
        ///     Validate the authentication header sent by Sinch.
        /// </summary>
        /// <param name="method">HTTP method of the incoming request.</param>
        /// <param name="path">Path of the incoming request.</param>
        /// <param name="headers">All HTTP headers from the incoming request.</param>
        /// <param name="body">Raw request body string.</param>
        /// <returns><c>true</c> if the signature is valid.</returns>
        bool ValidateAuthenticationHeader(
            HttpMethod method,
            string path,
            IEnumerable<KeyValuePair<string, IEnumerable<string>>> headers,
            string body);

        /// <summary>
        ///     Validate the authentication header sent by Sinch.
        ///     Use this overload with ASP.NET Core <c>request.Headers</c>.
        /// </summary>
        /// <param name="method">HTTP method of the incoming request.</param>
        /// <param name="path">Path of the incoming request.</param>
        /// <param name="headers">All HTTP headers from the incoming request.</param>
        /// <param name="body">Raw request body string.</param>
        /// <returns><c>true</c> if the signature is valid.</returns>
        bool ValidateAuthenticationHeader(
            HttpMethod method,
            string path,
            IEnumerable<KeyValuePair<string, StringValues>> headers,
            string body);

        /// <summary>
        ///     Serialize a <see cref="SinchEventResponse" /> (SVAML) to a JSON string suitable for returning
        ///     as the HTTP response body to an ICE, ACE, or PIE event.
        /// </summary>
        /// <param name="response">The SVAML response object to serialize.</param>
        /// <returns>JSON string representation of the response.</returns>
        string SerializeResponse(SinchEventResponse response);
    }
}
