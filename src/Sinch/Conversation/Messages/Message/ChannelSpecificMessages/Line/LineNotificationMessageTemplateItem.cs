using System.Text.Json.Serialization;

namespace Sinch.Conversation.Messages.Message.ChannelSpecificMessages.Line
{
    /// <summary>
    ///     Template item.
    /// </summary>
    public sealed class LineNotificationMessageTemplateItem
    {
        /// <summary>
        ///     Item key. See <see href="https://developers.line.biz/en/docs/partner-docs/line-notification-messages/template/?r=jp#items">LINE documentation</see> for available keys.
        /// </summary>
        [JsonPropertyName("item_key")]
        public required string ItemKey { get; set; }

        /// <summary>
        ///     Item value. Maximum length: 300.
        /// </summary>
        [JsonPropertyName("content")]
        public required string Content { get; set; }
    }
}
