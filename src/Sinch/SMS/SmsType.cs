using System.Text.Json.Serialization;
using Sinch.Core;

namespace Sinch.SMS
{
    [JsonConverter(typeof(EnumRecordJsonConverter<SmsType>))]
    public record SmsType(string Value) : EnumRecord(Value)
    {
        public static readonly SmsType MtText = new ("mt_text");
        public static readonly SmsType MtBinary = new ("mt_binary");
    }
}
