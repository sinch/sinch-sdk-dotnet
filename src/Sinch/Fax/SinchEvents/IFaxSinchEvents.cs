using System.Collections.Generic;
using System.Text.Json;

namespace Sinch.Fax.SinchEvents
{
    /// <summary>
    ///     Fax Sinch Events service. Provides helpers for parsing and validating
    ///     incoming Sinch event payloads delivered by the Fax API.
    /// </summary>
    public interface IFaxSinchEvents
    {
        internal JsonSerializerOptions JsonSerializerOptions { get; }

        /// <summary>
        ///     Parse a Fax Sinch event from a JSON payload.
        /// </summary>
        /// <param name="json">The raw JSON payload from the Sinch event request body.</param>
        /// <returns>
        ///     Parsed Fax Sinch event. Use pattern matching to handle specific event types:
        ///     <list type="bullet">
        ///         <item><description><see cref="IncomingFaxEvent"/> — An inbound fax has been received.</description></item>
        ///         <item><description><see cref="CompletedFaxEvent"/> — An outbound fax has completed.</description></item>
        ///     </list>
        /// </returns>
        /// <exception cref="System.Text.Json.JsonException">Thrown when JSON is invalid or cannot be deserialized.</exception>
        /// <exception cref="System.InvalidOperationException">Thrown when event type is unknown or deserialization fails.</exception>
        IFaxSinchEvent ParseEvent(string json);

        /// <summary>
        ///     Validates the Sinch event authentication header.
        /// </summary>
        /// <param name="headers">The HTTP headers from the incoming Sinch event request.</param>
        /// <param name="body">The raw request body.</param>
        /// <returns>
        ///     Always returns <c>true</c>. The Fax API does not define an authentication header
        ///     validation scheme, so no signature verification is performed.
        /// </returns>
        bool ValidateAuthenticationHeader(IDictionary<string, IEnumerable<string>> headers, string body);
    }
}
