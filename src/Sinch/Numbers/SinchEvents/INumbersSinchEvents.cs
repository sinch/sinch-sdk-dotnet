using System.IO;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Sinch.Numbers.SinchEvents
{
    /// <summary>
    ///     Numbers Sinch Events service. Provides helpers for parsing and validating
    ///     incoming Sinch event payloads delivered by the Numbers API.
    /// </summary>
    public interface INumbersSinchEvents
    {
        /// <summary>
        ///     For internal use: the <see cref="JsonSerializerOptions" /> used to deserialize Numbers events.
        /// </summary>
        JsonSerializerOptions JsonSerializerOptions { get; }

        /// <summary>
        ///     Parses a Sinch Numbers event from a raw JSON string.
        /// </summary>
        /// <param name="json">The raw Sinch event payload.</param>
        /// <returns>The parsed <see cref="INumberSinchEvent" />.</returns>
        INumberSinchEvent ParseEvent(string json);

        /// <summary>
        ///     Parses a Sinch Numbers event from a stream.
        /// </summary>
        /// <param name="json">A stream containing the JSON payload from the Sinch event request body.</param>
        /// <param name="cancellationToken"></param>
        /// <returns>The parsed <see cref="INumberSinchEvent" />.</returns>
        /// <exception cref="System.InvalidOperationException">Thrown when deserialization fails.</exception>
        Task<INumberSinchEvent> ParseEventAsync(Stream json, CancellationToken cancellationToken = default);

        /// <summary>
        ///     Validates a Sinch Numbers event using your HMAC secret and the raw signature header value.
        /// </summary>
        /// <param name="hmacSecret">Your HMAC secret.</param>
        /// <param name="json">The JSON payload as a raw string.</param>
        /// <param name="signatureHeaderValue">The value of the <c>X-Sinch-Signature</c> header.</param>
        /// <returns>True if the validation is successful.</returns>
        bool ValidateAuthenticationHeader(string hmacSecret, string json, string signatureHeaderValue);

        /// <summary>
        ///     Validates a Sinch Numbers event using your HMAC secret and the full request headers collection.
        /// </summary>
        /// <param name="hmacSecret">Your HMAC secret.</param>
        /// <param name="json">The JSON payload as a raw string.</param>
        /// <param name="headers">The HTTP headers of the incoming Sinch event request.</param>
        /// <returns>True if the validation is successful.</returns>
        bool ValidateAuthenticationHeader(string hmacSecret, string json, HttpHeaders headers);
    }
}
