using System.Text;
using System.Text.Json.Serialization;

namespace Sinch.Voice
{
    /// <summary>
    ///     The cost per minute to call the destination number.
    /// </summary>
    public sealed class Rate
    {
        /// <summary>
        ///     The currency ID of the rate, for example, &#x60;USD&#x60;.
        /// </summary>
        [JsonPropertyName("currencyId")]
        public string CurrencyId { get; set; }


        /// <summary>
        ///     The amount.
        /// </summary>
        [JsonPropertyName("amount")]
        public decimal Amount { get; set; }


        /// <summary>
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class Rate {\n");
            sb.Append("  CurrencyId: ").Append(CurrencyId).Append("\n");
            sb.Append("  Amount: ").Append(Amount).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }
    }
}
