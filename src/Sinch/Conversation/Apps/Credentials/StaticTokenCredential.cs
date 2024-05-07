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
        public string Token { get; set; } = null!;
#endif
    }
}
