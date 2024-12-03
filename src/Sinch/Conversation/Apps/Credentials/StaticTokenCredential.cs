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

        public required string Token { get; set; }

    }
}
