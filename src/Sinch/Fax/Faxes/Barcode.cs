using System.Text.Json.Serialization;
using Sinch.Core;

namespace Sinch.Fax.Faxes
{
    [JsonConverter(typeof(EnumRecordJsonConverter<BarCodeType>))]
    public record BarCodeType(string Value) : EnumRecord(Value)
    {
        public static readonly BarCodeType Code128 = new("CODE_128");
        public static readonly BarCodeType DataMatrix = new("DATA_MATRIX");
    }

    /// <summary>
    /// The bar codes found in the fax. This field is populated when sinch detects bar codes on incoming faxes.
    /// </summary>
    public class Barcode
    {
        /// <summary>
        /// The type of barcode found.
        /// </summary>
        [JsonPropertyName("type")]
        public BarCodeType? Type { get; set; }

        /// <summary>
        /// The page number on which the barcode was found.
        /// </summary>
        [JsonPropertyName("page")]
        public int Page { get; set; }

        /// <summary>
        /// The information of the barcode.
        /// </summary>
        [JsonPropertyName("value")]
        public string? Value { get; set; }
    }
}
