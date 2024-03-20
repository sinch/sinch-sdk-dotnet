using System.Text.Json.Serialization;
using Sinch.Core;

namespace Sinch.Conversation
{
    /// <summary>
    ///     Represents the identifier of the channel you want to include.
    /// </summary>
    [JsonConverter(typeof(EnumRecordJsonConverter<ConversationChannel>))]
    public record ConversationChannel(string Value) : EnumRecord(Value)
    {
        /// <summary>
        ///     WhatsApp channel.
        /// </summary>
        public static readonly ConversationChannel WhatsApp = new("WHATSAPP");

        /// <summary>
        ///     RCS channel.
        /// </summary>
        public static readonly ConversationChannel Rcs = new("RCS");

        /// <summary>
        ///     SMS channel.
        /// </summary>
        public static readonly ConversationChannel Sms = new("SMS");

        /// <summary>
        ///     Messenger channel.
        /// </summary>
        public static readonly ConversationChannel Messenger = new("MESSENGER");

        /// <summary>
        ///     Viber channel.
        /// </summary>
        public static readonly ConversationChannel Viber = new("VIBER");

        /// <summary>
        ///     ViberBm channel.
        /// </summary>
        public static readonly ConversationChannel ViberBm = new("VIBERBM");

        /// <summary>
        ///     MMS channel.
        /// </summary>
        public static readonly ConversationChannel Mms = new("MMS");

        /// <summary>
        ///     Instagram channel.
        /// </summary>
        public static readonly ConversationChannel Instagram = new("INSTAGRAM");

        /// <summary>
        ///     Telegram channel.
        /// </summary>
        public static readonly ConversationChannel Telegram = new("TELEGRAM");

        /// <summary>
        ///     KakaoTalk channel.
        /// </summary>
        public static readonly ConversationChannel KakaoTalk = new("KAKAOTALK");

        /// <summary>
        ///     KakaoTalkChat channel.
        /// </summary>
        public static readonly ConversationChannel KakaoTalkChat = new("KAKAOTALKCHAT");

        /// <summary>
        ///     Line channel.
        /// </summary>
        public static readonly ConversationChannel Line = new("LINE");

        /// <summary>
        ///     WeChat channel.
        /// </summary>
        public static readonly ConversationChannel WeChat = new("WECHAT");

        /// <summary>
        ///     AppleBC channel.
        /// </summary>
        public static readonly ConversationChannel AppleBC = new("APPLEBC");

        /// <summary>
        ///     Channel has not been specified
        /// </summary>
        public static readonly ConversationChannel Unspecified = new("CHANNEL_UNSPECIFIED");
    }
}
