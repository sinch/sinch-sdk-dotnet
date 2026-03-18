using System.Text.Json.Serialization;

namespace Sinch.Conversation.Messages.Message.ChannelSpecificMessages.Line
{
    /// <summary>
    ///     A message type for sending LINE notification messages (template).
    /// </summary>
    public sealed class LineNotificationMessageTemplateChannelSpecificMessage
    {
        /// <summary>
        ///     Template key. See <see href="https://developers.line.biz/en/docs/partner-docs/line-notification-messages/template/?r=jp#templates">LINE documentation</see> for available keys.
        /// </summary>
        [JsonPropertyName("template_key")]
        public required string TemplateKey { get; set; }

        /// <summary>
        ///     Template body.
        /// </summary>
        [JsonPropertyName("body")]
        public LineNotificationMessageTemplateBody? Body { get; set; }
    }
}
