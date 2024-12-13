using System.Text.Json.Serialization;
using Sinch.Core;

namespace Sinch.Conversation.TemplatesV1
{
    [JsonConverter(typeof(EnumRecordJsonConverter<TemplateChannel>))]
    public record TemplateChannel(string Value) : EnumRecord(Value)
    {
        public static readonly TemplateChannel Unspecfied = new("UNSPECIFIED");
        public static readonly TemplateChannel Conversation = new("CONVERSATION");
        public static readonly TemplateChannel Messenger = new("MESSENGER");
        public static readonly TemplateChannel WhatsApp = new("WHATSAPP");
        public static readonly TemplateChannel Rcs = new("RCS");
        public static readonly TemplateChannel Sms = new("SMS");
        public static readonly TemplateChannel Viber = new("VIBER");
        public static readonly TemplateChannel ViberBm = new("VIBERVM");
        public static readonly TemplateChannel Telegram = new("TELEGRAM");
        public static readonly TemplateChannel Instagram = new("INSTAGRAM");
        public static readonly TemplateChannel KakaoTalk = new("KAKAOTALK");
        public static readonly TemplateChannel Applebc = new("APPLEBC");
    }
}
