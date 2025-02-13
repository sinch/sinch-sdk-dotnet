using System.Text.Json.Serialization;

namespace Sinch.Conversation.Messages.Message
{
    public class MediaCardMessage
    {
        [JsonPropertyName("url")]
        public string? Url { get; set; }

        [JsonPropertyName("caption")]
        public string? Caption { get; set; }
    }
}
