using System.Text;

namespace Sinch.Conversation.Messages.Message
{
    /// <summary>
    ///     A message containing only text.
    /// </summary>
    public sealed class TextMessage : IMessage
    {
        /// <summary>
        ///     The text to be sent.
        /// </summary>
#if NET7_0_OR_GREATER
        public required string Text { get; set; }
#else
        public string Text { get; set; }
#endif


        /// <summary>
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("class TextMessage {\n");
            sb.Append("  Text: ").Append(Text).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }
    }
}
