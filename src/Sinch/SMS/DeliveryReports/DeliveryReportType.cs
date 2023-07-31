using System;
using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;
using Sinch.Core;

namespace Sinch.SMS.DeliveryReports
{
    [JsonConverter(typeof(DeliveryReportTypeEnumConverter))]
    public enum DeliveryReportType
    {
        [EnumMember(Value = "delivery_report_sms")]
        Sms,

        [EnumMember(Value = "delivery_report_mms")]
        Mms
    }

    internal class DeliveryReportTypeEnumConverter : JsonConverter<DeliveryReportType>
    {
        public override DeliveryReportType Read(ref Utf8JsonReader reader, Type typeToConvert,
            JsonSerializerOptions options)
        {
            return Utils.ParseEnum<DeliveryReportType>(reader.GetString());
        }

        public override void Write(Utf8JsonWriter writer, DeliveryReportType value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(Utils.GetEnumString(value));
        }
    }

    [JsonConverter(typeof(DeliveryReportVerbosityTypeEnumConverter))]
    public enum DeliveryReportVerbosityType
    {
        [EnumMember(Value = "summary")]
        Summary,

        [EnumMember(Value = "full")]
        Full
    }

    internal class DeliveryReportVerbosityTypeEnumConverter : JsonConverter<DeliveryReportVerbosityType>
    {
        public override DeliveryReportVerbosityType Read(ref Utf8JsonReader reader, Type typeToConvert,
            JsonSerializerOptions options)
        {
            return Utils.ParseEnum<DeliveryReportVerbosityType>(reader.GetString());
        }

        public override void Write(Utf8JsonWriter writer, DeliveryReportVerbosityType value,
            JsonSerializerOptions options)
        {
            writer.WriteStringValue(Utils.GetEnumString(value));
        }
    }
    
    [JsonConverter(typeof(RecipientDeliveryReportTypeEnumConverter))]
    public enum RecipientDeliveryReportType
    {
        [EnumMember(Value = "recipient_delivery_report_sms")]
        Sms,
    }

    internal class RecipientDeliveryReportTypeEnumConverter : JsonConverter<RecipientDeliveryReportType>
    {
        public override RecipientDeliveryReportType Read(ref Utf8JsonReader reader, Type typeToConvert,
            JsonSerializerOptions options)
        {
            return Utils.ParseEnum<RecipientDeliveryReportType>(reader.GetString());
        }

        public override void Write(Utf8JsonWriter writer, RecipientDeliveryReportType value,
            JsonSerializerOptions options)
        {
            writer.WriteStringValue(Utils.GetEnumString(value));
        }
    }
}
