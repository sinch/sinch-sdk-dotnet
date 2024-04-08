using System.Text.Json.Serialization;

namespace Sinch.Voice.Hooks
{
    /// <summary>
    ///     This is the general callback used to send notifications. It's a POST request to the specified calling callback URL.
    ///     <br /><br />
    ///     If there is no response to the callback within the timeout period, the notification is discarded.
    /// </summary>
    public class NotificationEvent
    {
        /// <summary>
        ///     Must have the value notify.
        /// </summary>
        [JsonPropertyName("event")]
        public string Event { get; set; }

        /// <summary>
        ///     The unique ID assigned to this call.
        /// </summary>
        [JsonPropertyName("callId")]
        public string CallId { get; set; }

        /// <summary>
        ///     The current API version.
        /// </summary>
        [JsonPropertyName("version")]
        public int Version { get; set; }

        /// <summary>
        ///     The type of information communicated in the notification.
        /// </summary>
        [JsonPropertyName("type")]
        public string Type { get; set; }

        /// <summary>
        ///     A string that can be used to pass custom information related to the call.
        /// </summary>
        [JsonPropertyName("custom")]
        public string Custom { get; set; }
    }
}
