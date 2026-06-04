using System.Text.Json.Serialization;
using Sinch.Core;

namespace Sinch.Verification.SinchEvents
{
    [JsonConverter(typeof(EnumRecordJsonConverter<SmsDeliveryResult>))]
    public record SmsDeliveryResult(string Value) : EnumRecord(Value)
    {
        public static readonly SmsDeliveryResult Successful = new("Successful");
        public static readonly SmsDeliveryResult Failed = new("Failed");
    }
}
