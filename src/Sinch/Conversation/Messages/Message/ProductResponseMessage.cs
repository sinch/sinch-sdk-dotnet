using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Sinch.Conversation.Messages.Message
{
    public sealed class ProductResponseMessage
    {
        /// <summary>
        ///     The selected products.
        /// </summary>
        [JsonPropertyName("products")]
#if NET7_0_OR_GREATER
       public required List<ProductItem> Products { get; set; }
#else
        public List<ProductItem> Products { get; set; } = null!;
#endif


        /// <summary>
        ///     Optional parameter. Text that may be sent with selected products.
        /// </summary>
        [JsonPropertyName("title")]
        public string? Title { get; set; }


        /// <summary>
        ///     Optional parameter. The catalog id that the selected products belong to.
        /// </summary>
        [JsonPropertyName("catalog_id")]
        public string? CatalogId { get; set; }


        /// <summary>
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append($"class {nameof(ProductResponseMessage)} {{\n");
            sb.Append($"  {nameof(Products)}: ").Append(Products).Append('\n');
            sb.Append($"  {nameof(Title)}: ").Append(Title).Append('\n');
            sb.Append($"  {nameof(CatalogId)}: ").Append(CatalogId).Append('\n');
            sb.Append("}\n");
            return sb.ToString();
        }
    }
}
