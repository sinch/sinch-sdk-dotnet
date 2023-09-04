using System.Text;

namespace Sinch.Conversation.Apps.Credentials
{
    /// <summary>
    ///     If you are including the Telegram Bot channel in the &#x60;channel_identifier&#x60; property, you must include this object.
    /// </summary>
    public sealed class TelegramCredentials
    {
        /// <summary>
        ///     The token for the Telegram bot to which you are connecting.
        /// </summary>
#if NET7_0_OR_GREATER
        public required string Token { get; set; }
#else
        public string Token { get; set; }
#endif
        

        /// <summary>
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class TelegramCredentials {\n");
            sb.Append("  Token: ").Append(Token).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }

    }
}
