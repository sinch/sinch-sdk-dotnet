using System.Text;

namespace Sinch.Conversation.Apps.Credentials
{   /// <summary>
    ///     It consists of a username and a password.
    /// </summary>
    public sealed class BasicAuthCredential
    {
        /// <summary>
        ///     Basic auth password.
        /// </summary>
#if NET7_0_OR_GREATER
        public required string Password { get; set; }
#else
        public string Password { get; set; }
#endif
        

        /// <summary>
        ///     Basic auth username.
        /// </summary>
#if NET7_0_OR_GREATER
        public required string Username { get; set; }
#else
        public string Username { get; set; }
#endif
        

        /// <summary>
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class BasicAuthCredential {\n");
            sb.Append("  Password: ").Append(Password).Append("\n");
            sb.Append("  Username: ").Append(Username).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }

    }
}
