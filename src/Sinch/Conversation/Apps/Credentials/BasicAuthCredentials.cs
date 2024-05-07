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
#if NET7_0_OR_GREATER
        public required string Password { get; set; }
#else
        public string Password { get; set; } = null!;
#endif


        /// <summary>
        ///     Basic auth username.
        /// </summary>
#if NET7_0_OR_GREATER
        public required string Username { get; set; }
#else
        public string Username { get; set; } = null!;
#endif
    }
}
