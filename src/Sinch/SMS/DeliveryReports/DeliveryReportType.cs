using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Sinch.SMS.DeliveryReports
{
    [JsonConverter(typeof(DeliveryReportTypeEnumConverter))]
    public enum DeliveryReportType
    {
        Summary,
        Full
    }

    internal class DeliveryReportTypeEnumConverter : JsonConverter<DeliveryReportType>
    {
        public override DeliveryReportType Read(ref Utf8JsonReader reader, Type typeToConvert,
            JsonSerializerOptions options)
        {
            return Enum.Parse<DeliveryReportType>(reader.GetString()!, true);
        }

        public override void Write(Utf8JsonWriter writer, DeliveryReportType value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString().ToLowerInvariant());
        }
    }
}
