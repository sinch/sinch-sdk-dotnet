using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Sinch.Core;
using Sinch.Verification.Common;

namespace Sinch.Verification.Start.Request
{
    public sealed class StartSmsVerificationRequest : StartVerificationRequestBase
    {
        /// <summary>
        ///     The type of the verification request. Set to SMS.
        /// </summary>
        [JsonInclude]
        public override VerificationMethodEx Method { get; } = VerificationMethodEx.Sms;

        /// <summary>
        ///     Value of [Accept-Language](https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Accept-Language) header is used to determine the language of an SMS message.
        /// </summary>
        [JsonIgnore]
        public string? AcceptLanguage { get; set; }

        /// <summary>
        ///     The SMS template must include a placeholder {{CODE}} where the verification code will be inserted, and it can otherwise be customized as desired.
        /// </summary>
        [JsonPropertyName("template")]
        public string? Template { get; set; }

        /// <summary>
        ///     Accepted values for the type of code to be generated are Numeric, Alpha, and Alphanumeric.
        /// </summary>
        [JsonPropertyName("codeType")]
        public CodeType? CodeType { get; set; }

        [JsonPropertyName("expiry")]
        [JsonConverter(typeof(TimeOnlyJsonConverter))]
        public TimeOnly? Expiry { get; set; }
    }

    public class TimeOnlyJsonConverter : JsonConverter<TimeOnly?>
    {
        public override TimeOnly? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var value = reader.GetString();
            if (string.IsNullOrEmpty(value))
            {
                return null;
            }

            var isParsed = TimeOnly.TryParseExact(value, "HH:mm:ss", out TimeOnly time);
            return isParsed ? time : null;
        }

        public override void Write(Utf8JsonWriter writer, TimeOnly? value, JsonSerializerOptions options)
        {
            var result = value.HasValue ? $"{value.Value:HH:mm:ss}" : null;
            JsonSerializer.Serialize(writer, result, options);
        }
    }

    [JsonConverter(typeof(EnumRecordJsonConverter<CodeType>))]
    public record CodeType(string Value) : EnumRecord(Value)
    {
        public static readonly CodeType Numeric = new("Numeric");
        public static readonly CodeType Alpha = new("Alpha");
        public static readonly CodeType Alphanumeric = new("Alphanumeric");
    }
}
