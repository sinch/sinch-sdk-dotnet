using System.Text.Json.Serialization;

namespace Sinch.Conversation.Messages.Message.ChannelSpecificMessages.Line
{
    /// <summary>
    ///     Template button.
    /// </summary>
    public sealed class LineNotificationMessageTemplateButton
    {
        /// <summary>
        ///     Button key. See <see href="https://developers.line.biz/en/docs/partner-docs/line-notification-messages/template/?r=jp#buttons">LINE documentation</see> for available keys.
        /// </summary>
        [JsonPropertyName("button_key")]
        public required string ButtonKey { get; set; }

        /// <summary>
        ///     Button URL. Maximum length: 1000.
        /// </summary>
        [JsonPropertyName("url")]
        public required string Url { get; set; }
    }
}
