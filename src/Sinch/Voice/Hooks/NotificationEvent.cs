using System.Text;
using System.Text.Json.Serialization;

namespace Sinch.Voice.Hooks
{
    /// <summary>
    ///     This is the general callback used to send notifications. It's a POST request to the specified calling callback URL.
    ///     <br /><br />
    ///     If there is no response to the callback within the timeout period, the notification is discarded.
    /// </summary>
    public sealed class NotificationEvent : IVoiceEvent
    {
        /// <summary>
        ///     Must have the value notify.
        /// </summary>
        [JsonPropertyName("event")]
        internal override EventType Event { get; set; } = EventType.NotificationEvent;

        /// <summary>
        ///     The unique ID assigned to this call.
        /// </summary>
        [JsonPropertyName("callid")]
        public string? CallId { get; set; }

        /// <summary>
        ///     Used in some types of events, it presents the unique Conference ID assigned to this call.
        /// </summary>
        [JsonPropertyName("conferenceId")]
        public string? ConferenceId { get; set; }

        /// <summary>
        ///     The current API version.
        /// </summary>
        [JsonPropertyName("version")]
        public int? Version { get; set; }

        /// <summary>
        ///     The type of information communicated in the notification.
        /// </summary>
        [JsonPropertyName("type")]
        public string? Type { get; set; }

        /// <summary>
        ///     A string that can be used to pass custom information related to the call.
        /// </summary>
        [JsonPropertyName("custom")]
        public string? Custom { get; set; }


        /// <inheritdoc cref="AnsweringMachineDetection"/>
        [JsonPropertyName("amd")]
        public AnsweringMachineDetection? Amd { get; set; }

        /// <summary>
        ///     Used in some types of events, it presents the destination of the generated recording or transcription files.
        /// </summary>
        [JsonPropertyName("destination")]
        public string? Destination { get; set; }

        /// <summary>
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append($"class {nameof(NotificationEvent)} {{\n");
            sb.Append($"  {nameof(Event)}: ").Append(Event).Append('\n');
            sb.Append($"  {nameof(Type)}: ").Append(Type).Append('\n');
            sb.Append($"  {nameof(Destination)}: ").Append(Destination).Append('\n');
            sb.Append($"  {nameof(Amd)}: ").Append(Amd).Append('\n');
            sb.Append($"  {nameof(Custom)}: ").Append(Custom).Append('\n');
            sb.Append("}\n");
            return sb.ToString();
        }
    }
}
