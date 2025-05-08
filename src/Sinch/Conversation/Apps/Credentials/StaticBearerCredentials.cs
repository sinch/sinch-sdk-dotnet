namespace Sinch.Conversation.Apps.Credentials
{
    /// <summary>
    ///     This object is required for channels which use a bearer-type of credential for authentication.
    /// </summary>
    public sealed class StaticBearerCredentials
    {
        /// <summary>
        ///     The claimed identity for the channel.
        /// </summary>
#if NET7_0_OR_GREATER
        public required string ClaimedIdentity { get; set; }
#else
        public string ClaimedIdentity { get; set; } = null!;
#endif


        /// <summary>
        ///     The static bearer token for the channel.
        /// </summary>
#if NET7_0_OR_GREATER
        public required string Token { get; set; }
#else
        public string Token { get; set; } = null!;
#endif
    }
}
