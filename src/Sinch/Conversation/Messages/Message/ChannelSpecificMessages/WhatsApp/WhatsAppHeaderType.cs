using System.Text.Json.Serialization;
using Sinch.Core;

namespace Sinch.Conversation.Messages.Message.ChannelSpecificMessages.WhatsApp
{
    [JsonConverter(typeof(EnumRecordJsonConverter<WhatsAppHeaderType>))]
    public record WhatsAppHeaderType(string Value) : EnumRecord(Value)
    {
        public static readonly WhatsAppHeaderType Video = new("video");
        public static readonly WhatsAppHeaderType Image = new("image");
        public static readonly WhatsAppHeaderType Text = new("text");
        public static readonly WhatsAppHeaderType Document = new("document");
    }
}
