using System.Text.Json.Serialization;
using Sinch.Core;

namespace Sinch.Verification.Common
{
    /// <summary>
    ///     The method of the verification request.
    /// </summary>
    /// <param name="Value"></param>
    [JsonConverter(typeof(EnumRecordJsonConverter<VerificationMethod>))]
    public record VerificationMethod(string Value) : EnumRecord(Value)
    {
        public static readonly VerificationMethod Sms = new("sms");
        public static readonly VerificationMethod FlashCall = new("flashCall");
        public static readonly VerificationMethod Callout = new("callout");
        
    }
}
