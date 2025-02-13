namespace Sinch.Conversation.Apps.Credentials
{
    /// <summary>
    ///     It consists of a username and a password.
    /// </summary>
    public sealed class BasicAuthCredential
    {
        /// <summary>
        ///     Basic auth password.
        /// </summary>

        public required string Password { get; set; }



        /// <summary>
        ///     Basic auth username.
        /// </summary>

        public required string Username { get; set; }

    }
}
