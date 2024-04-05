using System.Text.Json.Serialization;
using Sinch.Core;

namespace Sinch.SMS
{
    [JsonConverter(typeof(EnumRecordJsonConverter<SmsType>))]
    public record SmsType(string Value) : EnumRecord(Value)
    {
        /// <summary>
        ///     Regular SMS
        /// </summary>
        public static readonly SmsType MtText = new("mt_text");

        /// <summary>
        ///     SMS in <see href="https://community.sinch.com/t5/Glossary/Binary-SMS/ta-p/7470">binary</see> format.
        /// </summary>
        public static readonly SmsType MtBinary = new("mt_binary");

        /// <summary>
        ///     MMS
        /// </summary>
        public static readonly SmsType MtMedia = new("mt_media");
    }
}
