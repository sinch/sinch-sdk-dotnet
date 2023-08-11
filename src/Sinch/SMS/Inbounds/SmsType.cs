using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using Sinch.Core;

namespace Sinch.SMS.Inbounds
{
    [JsonConverter(typeof(SinchEnumConverter<SmsType>))]
    public enum SmsType
    {
        [EnumMember(Value = "mo_text")]
        Text,

        [EnumMember(Value = "mo_binary")]
        Binary
    }
}
