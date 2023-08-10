using System;
using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;
using Sinch.Core;

namespace Sinch.SMS
{
    [JsonConverter(typeof(SinchEnumConverter<SmsType>))]
    public enum SmsType
    {
        [EnumMember(Value = "mt_text")]
        MtText,

        [EnumMember(Value = "mt_binary")]
        MtBinary
    }
}
