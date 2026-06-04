using System.Text.Json.Serialization;
using Sinch.Core;

namespace Sinch.Verification.Common
{
    [JsonConverter(typeof(EnumRecordJsonConverter<SmsCodeType>))]
    public record SmsCodeType(string Value) : EnumRecord(Value)
    {
        /// <summary>Code contains numbers only.</summary>
        public static readonly SmsCodeType Numeric = new("Numeric");

        /// <summary>Code contains letters only.</summary>
        public static readonly SmsCodeType Alpha = new("Alpha");

        /// <summary>Code contains both numbers and letters.</summary>
        public static readonly SmsCodeType Alphanumeric = new("Alphanumeric");
    }
}
