using System.Text;

namespace Sinch.Conversation.Apps.Credentials
{
    /// <summary>
    ///     This object is required for channels which use a static token credential for authentication.
    /// </summary>
    public sealed class StaticTokenCredential
    {
        /// <summary>
        ///     The static token for the channel.
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
            sb.Append("class StaticTokenCredential {\n");
            sb.Append("  Token: ").Append(Token).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }

    }
}
