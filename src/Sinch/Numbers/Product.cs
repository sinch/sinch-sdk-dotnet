using System.Text.Json.Serialization;
using Sinch.Core;

namespace Sinch.Numbers
{
    /// <summary>
    ///     Represents the product options.
    /// </summary>
    [JsonConverter(typeof(EnumRecordJsonConverter<Product>))]
    public record Product(string Value) : EnumRecord(Value)
    {
        /// <summary>
        ///     The SMS product can use the number.
        /// </summary>
        public static readonly Product Sms = new("Sms");

        /// <summary>
        ///     The Voice product can use the number.
        /// </summary>
        public static readonly Product Voice = new("Voice");
    }
}
