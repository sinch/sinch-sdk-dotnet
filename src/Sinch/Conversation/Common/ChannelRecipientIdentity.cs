using System.Text.Json.Serialization;

namespace Sinch.Conversation.Common
{
    /// <summary>
    ///     A channel identity used to identify a recipient for lookup purposes (e.g. channel profile retrieval).
    ///     Unlike <see cref="ChannelIdentity" />, this type does not carry an <c>app_id</c> — the app context
    ///     is provided separately at the request level.
    /// </summary>
    public sealed class ChannelRecipientIdentity
    {
        /// <summary>
        ///     The channel recipient identity (e.g. a phone number for SMS, a user ID for Messenger).
        /// </summary>
        [JsonPropertyName("identity")]
        public string? Identity { get; set; }

        /// <summary>
        ///     The channel this identity belongs to.
        /// </summary>
        [JsonPropertyName("channel")]
        public ConversationChannel? Channel { get; set; }
    }
}
