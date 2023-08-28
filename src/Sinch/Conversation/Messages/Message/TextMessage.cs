﻿using System.Text;

namespace Sinch.Conversation.Messages.Message
{
    /// <summary>
    ///     A message containing only text.
    /// </summary>
    public sealed class TextMessage : IMessage
    {
        /// <summary>
        ///     A message containing only text.
        /// </summary>
        /// <param name="Text">Text</param>
        public TextMessage(string Text)
        {
            this.Text = Text;
        }

        /// <summary>
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class TextMessage {\n");
            sb.Append("  Text: ").Append(Text).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }

        /// <summary>Text</summary>
        public string Text { get; init; }
    }
}