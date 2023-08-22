using System.Text;

namespace Sinch.Conversation.Messages.Message
{
    /// <summary>
    ///     A message component for interactive messages, containing a product.
    /// </summary>
    public sealed class ProductItem : IListItem
    {
        /// <summary>
        ///     Required parameter. The ID for the product.
        /// </summary>
#if NET7_0_OR_GREATER
        public required string Id { get; set; }
#else
        public string Id { get; set; }
#endif


        /// <summary>
        ///     Required parameter. The marketplace to which the product belongs.
        /// </summary>
#if NET7_0_OR_GREATER
        public required string Marketplace { get; set; }
#else
        public string Marketplace { get; set; }
#endif


        /// <summary>
        ///     Output only. The quantity of the chosen product.
        /// </summary>
        public int Quantity { get; set; }


        /// <summary>
        ///     Output only. The price for one unit of the chosen product.
        /// </summary>
        public float ItemPrice { get; set; }


        /// <summary>
        ///     Output only. The currency of the item_price.
        /// </summary>
        public string Currency { get; set; }


        /// <summary>
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("class ProductItem {\n");
            sb.Append("  Id: ").Append(Id).Append("\n");
            sb.Append("  Marketplace: ").Append(Marketplace).Append("\n");
            sb.Append("  Quantity: ").Append(Quantity).Append("\n");
            sb.Append("  ItemPrice: ").Append(ItemPrice).Append("\n");
            sb.Append("  Currency: ").Append(Currency).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }
    }
}
