using System.Text;

namespace Sinch.Conversation.Messages.Message
{
    /// <summary>
    ///     A message component for interactive messages, containing a choice.
    /// </summary>
    public class ChoiceOld : IListItem
    {
        /// <summary>
        ///     Required parameter. Title for the choice item.
        /// </summary>
#if NET7_0_OR_GREATER
        public required string Title { get; set; }
#else
        public string Title { get; set; }
#endif


        /// <summary>
        ///     Optional parameter. The description (or subtitle) of this choice item.
        /// </summary>
        public string Description { get; set; }


        /// <summary>
        ///     Gets or Sets Media
        /// </summary>
        public MediaMessage Media { get; set; }


        /// <summary>
        ///     Optional parameter. Postback data that will be returned in the MO if the user selects this option.
        /// </summary>
        public string PostbackData { get; set; }


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
