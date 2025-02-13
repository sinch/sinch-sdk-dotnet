using System.Text.Json.Serialization;

namespace Sinch.Conversation.Hooks.Models
{
    /// <summary>
    ///     This functionality is currently only available for the Messenger and Instagram channels.
    /// </summary>
    public class ShortlinkActivatedEvent
    {
        /// <summary>
        /// Refers to the payload previously configured to be sent in the postback.
        /// </summary>
        [JsonPropertyName("payload")]
        public string? Payload { get; set; }

        /// <summary>
        /// Only relevant for the Instagram channel.
        /// </summary>
        [JsonPropertyName("title")]
        public string? Title { get; set; }

        /// <summary>
        /// The ref parameter from the shortlink the user visited.
        /// </summary>
        [JsonPropertyName("ref")]
        public string? Ref { get; set; }

        /// <summary>
        /// Defaults to "SHORTLINK" for this type of event.
        /// </summary>
        [JsonPropertyName("source")]
        public string? Source { get; set; }

        /// <summary>
        /// The identifier for the referral. For Instagram and Messenger shortlinks, this is always set to "OPEN_THREAD".
        /// </summary>
        [JsonPropertyName("type")]
        public string? Type { get; set; }

        /// <summary>
        /// Set to true if target channel's conversation thread already existed at the moment the shortlink was visited.
        /// Set to false if a new conversation thread began when the shortlink was visited.
        /// </summary>
        [JsonPropertyName("existing_thread")]
        public bool? ExistingThread { get; set; }
    }
}
