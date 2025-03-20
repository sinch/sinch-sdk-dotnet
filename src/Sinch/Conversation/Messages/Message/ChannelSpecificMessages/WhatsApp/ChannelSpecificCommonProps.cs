using System.Text.Json.Serialization;

namespace Sinch.Conversation.Messages.Message.ChannelSpecificMessages.WhatsApp
{
    public class ChannelSpecificCommonProps
    {
        /// <summary>
        ///     Gets or Sets Header
        /// </summary>
        [JsonPropertyName("header")]
        public IFlowChannelSpecificMessageHeader? Header { get; set; }

        /// <summary>
        ///     Gets or Sets Body
        /// </summary>
        [JsonPropertyName("body")]
        public WhatsAppInteractiveBody? Body { get; set; }


        /// <summary>
        ///     Gets or Sets Footer
        /// </summary>
        [JsonPropertyName("footer")]
        public WhatsAppInteractiveFooter? Footer { get; set; }
    }
}
