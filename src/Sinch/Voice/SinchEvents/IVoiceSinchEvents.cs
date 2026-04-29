using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Sinch.Voice.SinchEvents
{
    /// <summary>
    ///     Voice Sinch Events service. Provides helpers for parsing
    ///     incoming Voice Sinch event payloads delivered by the Voice API.
    /// </summary>
    public interface IVoiceSinchEvents
    {
        JsonSerializerOptions JsonSerializerOptions { get; }

        /// <summary>
        ///     Parses a Voice Sinch event from a JSON string.
        /// </summary>
        /// <param name="json">The raw JSON payload from the Voice Sinch event request body.</param>
        /// <returns>
        ///     Parsed Voice Sinch event. Use pattern matching to handle specific event types:
        ///     <list type="bullet">
        ///         <item><description><see cref="IncomingCallEvent"/> — An incoming call (ICE).</description></item>
        ///         <item><description><see cref="AnsweredCallEvent"/> — A call was answered (ACE).</description></item>
        ///         <item><description><see cref="DisconnectedCallEvent"/> — A call was disconnected (DICE).</description></item>
        ///         <item><description><see cref="PromptInputEvent"/> — Prompt input received (PIE).</description></item>
        ///         <item><description><see cref="NotificationEvent"/> — A general notification (notify).</description></item>
        ///     </list>
        /// </returns>
        /// <exception cref="System.Text.Json.JsonException">Thrown when JSON is invalid or cannot be deserialized.</exception>
        /// <exception cref="System.InvalidOperationException">Thrown when the event type is unknown or deserialization fails.</exception>
        IVoiceSinchEvent ParseEvent(string json);

        /// <summary>
        ///     Parses a Voice Sinch event from a stream.
        /// </summary>
        /// <param name="json">A stream containing the JSON payload from the Voice Sinch event request body.</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Parsed Voice Sinch event.</returns>
        /// <exception cref="System.InvalidOperationException">Thrown when the event type is unknown or deserialization fails.</exception>
        Task<IVoiceSinchEvent> ParseEventAsync(Stream json, CancellationToken cancellationToken = default);

        /// <summary>
        ///     Validates the authentication header of an incoming Voice Sinch event request.
        /// </summary>
        /// <param name="method">HTTP method of the incoming request.</param>
        /// <param name="path">Path of the incoming request (e.g. <c>/webhooks/voice</c>).</param>
        /// <param name="headers">Headers from the incoming request.</param>
        /// <param name="body">Raw request body string.</param>
        /// <returns><see langword="true"/> if the computed signature matches the Authorization header; otherwise <see langword="false"/>.</returns>
        bool ValidateAuthenticationHeader(HttpMethod method, string path,
            IDictionary<string, IEnumerable<string>> headers,
            string body);
    }
}
