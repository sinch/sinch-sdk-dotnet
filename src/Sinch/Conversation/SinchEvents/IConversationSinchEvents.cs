using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Sinch.Conversation.SinchEvents
{
    /// <summary>
    ///     Conversation Sinch Events service. Provides helpers for parsing and validating
    ///     incoming Sinch event payloads delivered by the Conversation API.
    /// </summary>
    public interface IConversationSinchEvents
    {
        /// <summary>
        ///     The <see cref="JsonSerializerOptions" /> used to deserialize Conversation events.
        /// </summary>
        JsonSerializerOptions JsonSerializerOptions { get; }
        
        /// <summary>
        ///     Validates the Sinch event HMAC authentication header.
        /// </summary>
        /// <param name="headers">The HTTP headers from the incoming Sinch event request as single-value entries.</param>
        /// <param name="body">The raw request body.</param>
        /// <param name="secret">The event destination secret used to generate the HMAC signature.</param>
        /// <returns>True if the produced signature matches the header value.</returns>
        bool ValidateAuthenticationHeader(IDictionary<string, string> headers, string body, string secret);

        /// <summary>
        ///     Validates the Sinch event HMAC authentication header.
        /// </summary>
        /// <param name="headers">The HTTP headers from the incoming Sinch event request.</param>
        /// <param name="body">The raw request body.</param>
        /// <param name="secret">The event destination secret used to generate the HMAC signature.</param>
        /// <returns>True if the produced signature matches the header value.</returns>
        bool ValidateAuthenticationHeader(IReadOnlyDictionary<string, IEnumerable<string>> headers, string body,
            string secret);

        /// <summary>
        ///     Parses a Conversation Sinch event from a JSON string.
        /// </summary>
        /// <param name="json">The raw JSON payload from the Sinch event request body.</param>
        /// <returns>
        ///     Parsed Conversation Sinch event. Use pattern matching to handle specific event types:
        ///     <list type="bullet">
        ///         <item><description><see cref="MessageInboundEvent"/> — An inbound message from an end user.</description></item>
        ///         <item><description><see cref="MessageDeliveryReceiptEvent"/> — A message delivery receipt.</description></item>
        ///         <item><description><see cref="MessageSubmitEvent"/> — A message submission notification.</description></item>
        ///         <item><description><see cref="DeliveryEvent"/> — An event delivery receipt.</description></item>
        ///         <item><description><see cref="InboundEvent"/> — An inbound event from an end user.</description></item>
        ///         <item><description><see cref="CapabilityEvent"/> — A channel capability lookup result.</description></item>
        ///         <item><description><see cref="ContactCreateEvent"/> — A contact was created.</description></item>
        ///         <item><description><see cref="ContactDeleteEvent"/> — A contact was deleted.</description></item>
        ///         <item><description><see cref="ContactMergeEvent"/> — Two contacts were merged.</description></item>
        ///         <item><description><see cref="ContactUpdateEvent"/> — A contact was updated.</description></item>
        ///         <item><description><see cref="ConversationStartEvent"/> — A conversation was started.</description></item>
        ///         <item><description><see cref="ConversationStopEvent"/> — A conversation was stopped.</description></item>
        ///         <item><description><see cref="ConversationDeleteEvent"/> — A conversation was deleted.</description></item>
        ///         <item><description><see cref="ChannelEvent"/> — A channel-specific event.</description></item>
        ///         <item><description><see cref="OptInEvent"/> — An opt-in notification.</description></item>
        ///         <item><description><see cref="OptOutEvent"/> — An opt-out notification.</description></item>
        ///         <item><description><see cref="SmartConversationsEvent"/> — A Smart Conversations analysis result.</description></item>
        ///     </list>
        /// </returns>
        /// <exception cref="System.Text.Json.JsonException">Thrown when JSON is invalid or cannot be deserialized.</exception>
        /// <exception cref="System.InvalidOperationException">Thrown when the event type is unknown or deserialization fails.</exception>
        IConversationSinchEvent ParseEvent(string json);

        /// <summary>
        ///     Parses a Conversation Sinch event from a stream.
        /// </summary>
        /// <param name="json">A stream containing the JSON payload from the Sinch event request body.</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Parsed Conversation Sinch event.</returns>
        /// <exception cref="System.InvalidOperationException">Thrown when the event type is unknown or deserialization fails.</exception>
        Task<IConversationSinchEvent> ParseEventAsync(Stream json, CancellationToken cancellationToken = default);
    }
}
