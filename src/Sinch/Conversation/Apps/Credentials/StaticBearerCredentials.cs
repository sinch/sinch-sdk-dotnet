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

        public required string ClaimedIdentity { get; set; }



        /// <summary>
        ///     The static bearer token for the channel.
        /// </summary>

        public required string Token { get; set; }

    }
}
