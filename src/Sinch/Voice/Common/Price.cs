using System.Text;
using System.Text.Json.Serialization;

namespace Sinch.Voice.Common
{
    public sealed class Price
    {
        /// <summary>
        ///     Gets or Sets CurrencyId
        /// </summary>
        [JsonPropertyName("currencyId")]
        public string? CurrencyId { get; set; }


        /// <summary>
        ///     Gets or Sets Amount
        /// </summary>
        [JsonPropertyName("amount")]
        public float? Amount { get; set; }


        /// <summary>
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append($"class {nameof(Price)} {{\n");
            sb.Append($"  {nameof(CurrencyId)}: ").Append(CurrencyId).Append('\n');
            sb.Append($"  {nameof(Amount)}: ").Append(Amount).Append('\n');
            sb.Append("}\n");
            return sb.ToString();
        }
    }
}
