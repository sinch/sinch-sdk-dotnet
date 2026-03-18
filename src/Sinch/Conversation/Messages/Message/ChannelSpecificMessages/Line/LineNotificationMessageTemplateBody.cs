using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Sinch.Conversation.Messages.Message.ChannelSpecificMessages.Line
{
    /// <summary>
    ///     Template body.
    /// </summary>
    public sealed class LineNotificationMessageTemplateBody
    {
        /// <summary>
        ///     Emphasized item.
        /// </summary>
        [JsonPropertyName("emphasized_item")]
        public LineNotificationMessageTemplateEmphasizedItem? EmphasizedItem { get; set; }

        /// <summary>
        ///     List of template items. Maximum 15 items.
        /// </summary>
        [JsonPropertyName("items")]
        public List<LineNotificationMessageTemplateItem>? Items { get; set; }

        /// <summary>
        ///     List of template buttons. Maximum 2 buttons.
        /// </summary>
        [JsonPropertyName("buttons")]
        public List<LineNotificationMessageTemplateButton>? Buttons { get; set; }
    }
}
