using System;
using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;
using Sinch.Core;

namespace Sinch.SMS.DeliveryReports
{
    [JsonConverter(typeof(SinchEnumConverter<DeliveryReportType>))]
    public enum DeliveryReportType
    {
        [EnumMember(Value = "delivery_report_sms")]
        Sms,

        [EnumMember(Value = "delivery_report_mms")]
        Mms
    }

    [JsonConverter(typeof(SinchEnumConverter<DeliveryReportVerbosityType>))]
    public enum DeliveryReportVerbosityType
    {
        [EnumMember(Value = "summary")]
        Summary,

        [EnumMember(Value = "full")]
        Full
    }
    
    [JsonConverter(typeof(SinchEnumConverter<RecipientDeliveryReportType>))]
    public enum RecipientDeliveryReportType
    {
        [EnumMember(Value = "recipient_delivery_report_sms")]
        Sms,
    }
}
