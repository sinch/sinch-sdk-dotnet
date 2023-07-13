using System;
using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;
using Sinch.Core;

namespace Sinch.SMS.Inbounds
{
    [JsonConverter(typeof(SmsTypeEnumConverter))]
    public enum SmsType
    {
        [EnumMember(Value = "mo_text")]
        Text,

        [EnumMember(Value = "mo_binary")]
        Binary
    }

    internal class SmsTypeEnumConverter : JsonConverter<SmsType>
    {
        public override SmsType Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return Utils.ParseEnum<SmsType>(reader.GetString());
        }

        public override void Write(Utf8JsonWriter writer, SmsType value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(Utils.GetEnumString(value));
        }
    }
}
