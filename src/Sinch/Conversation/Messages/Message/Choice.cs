using System.Text.Json.Serialization;

namespace Sinch.Conversation.Messages.Message
{
    public class Choice
    {
        /// <summary>
        ///     Message for triggering a call.
        /// </summary>
        [JsonPropertyName("call_message")]
        public CallMessage? CallMessage { get; set; }

        /// <summary>
        ///     Message containing geographic location.
        /// </summary>
        [JsonPropertyName("location_message")]
        public LocationMessage? LocationMessage { get; set; }

        /// <summary>
        ///     An optional field. This data will be returned in the ChoiceResponseMessage.
        ///     The default is message_id_{text, title}.
        /// </summary>
        [JsonPropertyName("postback_data")]
        public string? PostbackData { get; set; }

        /// <summary>
        ///     A message containing only text.
        /// </summary>
        [JsonPropertyName("text_message")]
        public TextMessage? TextMessage { get; set; }

        /// <summary>
        ///     A generic URL message.
        /// </summary>
        [JsonPropertyName("url_message")]
        public UrlMessage? UrlMessage { get; set; }
    }
}
