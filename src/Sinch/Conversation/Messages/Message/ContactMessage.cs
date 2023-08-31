using System.Text;

namespace Sinch.Conversation.Messages.Message
{
    
    public class ContactMessage
{   /// <summary>
        ///     Gets or Sets ChoiceResponseMessage
        /// </summary>
        public ChoiceResponseMessage ChoiceResponseMessage { get; set; }
        

        /// <summary>
        ///     Gets or Sets FallbackMessage
        /// </summary>
        public FallbackMessage FallbackMessage { get; set; }
        

        /// <summary>
        ///     Gets or Sets LocationMessage
        /// </summary>
        public LocationMessage LocationMessage { get; set; }
        

        /// <summary>
        ///     Gets or Sets MediaCardMessage
        /// </summary>
        public MediaCarouselMessage MediaCardMessage { get; set; }
        

        /// <summary>
        ///     Gets or Sets MediaMessage
        /// </summary>
        public MediaMessage MediaMessage { get; set; }
        

        /// <summary>
        ///     Gets or Sets ReplyTo
        /// </summary>
        public ReplyTo ReplyTo { get; set; }
        

        /// <summary>
        ///     Gets or Sets TextMessage
        /// </summary>
        public TextMessage TextMessage { get; set; }
        

        /// <summary>
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class ContactMessage {\n");
            sb.Append("  ChoiceResponseMessage: ").Append(ChoiceResponseMessage).Append("\n");
            sb.Append("  FallbackMessage: ").Append(FallbackMessage).Append("\n");
            sb.Append("  LocationMessage: ").Append(LocationMessage).Append("\n");
            sb.Append("  MediaCardMessage: ").Append(MediaCardMessage).Append("\n");
            sb.Append("  MediaMessage: ").Append(MediaMessage).Append("\n");
            sb.Append("  ReplyTo: ").Append(ReplyTo).Append("\n");
            sb.Append("  TextMessage: ").Append(TextMessage).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }
}
}

