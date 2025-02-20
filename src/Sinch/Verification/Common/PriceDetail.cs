using System.Text.Json.Serialization;

namespace Sinch.Verification.Common
{
    public sealed class PriceDetail
    {
        /// <summary>
        ///     ISO 4217 currency code
        /// </summary>
        [JsonPropertyName("currencyId")]
        public string? CurrencyId { get; set; }

        [JsonPropertyName("amount")]
        public double Amount { get; set; }
    }
}
