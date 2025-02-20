using System.Text;
using System.Text.Json.Serialization;

namespace Sinch.Fax.Faxes
{
    /// <summary>
    ///     
    /// </summary>
    public sealed class Money

    {
        /// <summary>
        ///     The 3-letter currency code defined in ISO 4217.
        /// </summary>
        [JsonPropertyName("currencyCode")]
        [JsonInclude]
        public string? CurrencyCode { get; private set; }


        /// <summary>
        ///     The amount with 4 decimals and decimal delimiter &#x60;.&#x60;.
        /// </summary>
        [JsonPropertyName("amount")]
        [JsonInclude]
        public float Amount { get; private set; }

        /// <summary>
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append($"class {nameof(Money)} {{\n");
            sb.Append($"  {nameof(CurrencyCode)}: ").Append(CurrencyCode).Append('\n');
            sb.Append($"  {nameof(Amount)}: ").Append(Amount).Append('\n');
            sb.Append("}\n");
            return sb.ToString();
        }
    }
}
