using System.Collections.Generic;
using System.Text;

namespace Sinch.Conversation.Messages.Message
{
    public class ChoiceMessage : IMessage
    {
        /// <summary>
        ///     The number of choices is limited to 10.
        /// </summary>
#if NET7_0_OR_GREATER
        public required List<Choice> Choices { get; set; }
#else
        public List<Choice> Choices { get; set; }
#endif


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
            StringBuilder sb = new StringBuilder();
            sb.Append("class ChoiceMessage {\n");
            sb.Append("  Choices: ").Append(Choices).Append("\n");
            sb.Append("  TextMessage: ").Append(TextMessage).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }
    }
}
