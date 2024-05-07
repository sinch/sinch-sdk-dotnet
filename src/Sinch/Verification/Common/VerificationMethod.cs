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
        public static readonly VerificationMethod FlashCall = new("flashcall");
        public static readonly VerificationMethod Callout = new("callout");
    }

    /// <summary>
    ///     The method of the verification request extended. Includes <see cref="Seamless"/>
    /// </summary>
    /// <param name="Value"></param>
    [JsonConverter(typeof(EnumRecordJsonConverter<VerificationMethodEx>))]
    public record VerificationMethodEx(string Value) : EnumRecord(Value)
    {
        public static readonly VerificationMethodEx Sms = new("sms");
        public static readonly VerificationMethodEx FlashCall = new("flashcall");
        public static readonly VerificationMethodEx Callout = new("callout");
        public static readonly VerificationMethodEx Seamless = new("seamless");
    }
}
