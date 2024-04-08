using System.Text.Json.Serialization;
using Sinch.Core;

namespace Sinch.Verification.Common
{
    public class Identity
    {
        /// <summary>
        ///     Creates an Identity with phone number.
        /// </summary>
        /// <param name="phoneNumber">an E.164-compatible phone number</param>
        /// <returns></returns>
        public static Identity Number(string phoneNumber)
        {
            return new Identity()
            {
                Type = IdentityType.Number,
                Endpoint = phoneNumber
            };
        }

        /// <summary>
        ///     Currently only number type is supported.
        /// </summary>
        [JsonPropertyName("type")]
        public IdentityType Type { get; set; }

        /// <summary>
        ///     For type number use an E.164-compatible phone number.
        /// </summary>
        [JsonPropertyName("endpoint")]
        public string Endpoint { get; set; }
    }

    [JsonConverter(typeof(EnumRecordJsonConverter<IdentityType>))]
    public record IdentityType(string Value) : EnumRecord(Value)
    {
        /// <summary>
        ///     Verification by SMS message with a PIN code.
        /// </summary>
        public static readonly IdentityType Number = new("number");
    }
}
