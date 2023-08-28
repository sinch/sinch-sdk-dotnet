using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using Sinch.Core;

namespace Sinch.Conversation
{
    /// <summary>
    /// The identifier of the channel you want to include. Must be one of the enum values.
    /// </summary>
    /// <value>The identifier of the channel you want to include. Must be one of the enum values.</value>
    [JsonConverter(typeof(SinchEnumConverter<ConversationChannel>))]
    public enum ConversationChannel
    {
        /// <summary>
        ///     Enum WHATSAPP for value: WHATSAPP
        /// </summary>
        [EnumMember(Value = "WHATSAPP")]
        WhatsApp = 1,

        /// <summary>
        /// Enum RCS for value: RCS
        /// </summary>
        [EnumMember(Value = "RCS")]
        Rcs = 2,

        /// <summary>
        ///     Enum SMS for value: SMS
        /// </summary>
        [EnumMember(Value = "SMS")]
        Sms = 3,

        /// <summary>
        ///     Enum MESSENGER for value: MESSENGER
        /// </summary>
        [EnumMember(Value = "MESSENGER")]
        Messenger = 4,

        /// <summary>
        ///     Enum VIBER for value: VIBER
        /// </summary>
        [EnumMember(Value = "VIBER")]
        Viber = 5,

        /// <summary>
        ///     Enum VIBERBM for value: VIBERBM
        /// </summary>
        [EnumMember(Value = "VIBERBM")]
        ViberBm = 6,

        /// <summary>
        ///     Enum MMS for value: MMS
        /// </summary>
        [EnumMember(Value = "MMS")]
        Mms = 7,

        /// <summary>
        ///     Enum INSTAGRAM for value: INSTAGRAM
        /// </summary>
        [EnumMember(Value = "INSTAGRAM")]
        Instagram = 8,

        /// <summary>
        ///     Enum TELEGRAM for value: TELEGRAM
        /// </summary>
        [EnumMember(Value = "TELEGRAM")]
        Telegram = 9,

        /// <summary>
        ///     Enum KAKAOTALK for value: KAKAOTALK
        /// </summary>
        [EnumMember(Value = "KAKAOTALK")]
        KakaoTalk = 10,

        /// <summary>
        ///     Enum KAKAOTALKCHAT for value: KAKAOTALKCHAT
        /// </summary>
        [EnumMember(Value = "KAKAOTALKCHAT")]
        KakaoTalkChat = 11,

        /// <summary>
        ///     Enum LINE for value: LINE
        /// </summary>
        [EnumMember(Value = "LINE")]
        Line = 12,

        /// <summary>
        ///     Enum WECHAT for value: WECHAT
        /// </summary>
        [EnumMember(Value = "WECHAT")]
        WeChat = 13
    }
}
