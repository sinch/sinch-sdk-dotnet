using System;
using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Sinch.Numbers
{
    [JsonConverter(typeof(StatusEnumConverter))]
    public enum ProvisioningStatus
    {
        [EnumMember(Value = "WAITING")]
        Waiting,

        [EnumMember(Value = "IN_PROGRESS")]
        InProgress,

        [EnumMember(Value = "FAILED")]
        Failed
    }

    internal sealed class StatusEnumConverter : JsonConverter<ProvisioningStatus>
    {
        public override ProvisioningStatus Read(ref Utf8JsonReader reader, Type typeToConvert,
            JsonSerializerOptions options)
        {
            return Enum.Parse<ProvisioningStatus>(reader.GetString()!, true);
        }

        public override void Write(Utf8JsonWriter writer, ProvisioningStatus value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }
    }
}
