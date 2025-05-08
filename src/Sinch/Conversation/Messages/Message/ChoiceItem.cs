using System.Text;
using System.Text.Json.Serialization;

namespace Sinch.Conversation.Messages.Message
{
    /// <summary>
    ///     A message component for interactive messages, containing a choice.
    /// </summary>
    public sealed class ChoiceItem : IListItem
    {
        /// <summary>
        ///     Required parameter. Title for the choice item.
        /// </summary>
        [JsonPropertyName("title")]
#if NET7_0_OR_GREATER
        public required string Title { get; set; }
#else
        public string Title { get; set; } = null!;
#endif


        /// <summary>
        ///     Optional parameter. The description (or subtitle) of this choice item.
        /// </summary>
        [JsonPropertyName("description")]
        public string? Description { get; set; }


        /// <summary>
        ///     Gets or Sets Media
        /// </summary>
        [JsonPropertyName("media")]
        public MediaMessage? Media { get; set; }


        /// <summary>
        ///     Optional parameter. Postback data that will be returned in the MO if the user selects this option.
        /// </summary>
        [JsonPropertyName("postback_data")]
        public string? PostbackData { get; set; }


        /// <summary>
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("class ChoiceItem {\n");
            sb.Append("  Title: ").Append(Title).Append("\n");
            sb.Append("  Description: ").Append(Description).Append("\n");
            sb.Append("  Media: ").Append(Media).Append("\n");
            sb.Append("  PostbackData: ").Append(PostbackData).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }
    }
}
