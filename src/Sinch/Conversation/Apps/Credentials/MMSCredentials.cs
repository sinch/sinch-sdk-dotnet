namespace Sinch.Conversation.Apps.Credentials
{
    /// <summary>
    ///     If you are including the MMS channel in the &#x60;channel_identifier&#x60; property, you must include this object.
    /// </summary>
    public sealed class MmsCredentials
    {
        /// <summary>
        ///     MMS Account ID.
        /// </summary>

        public required string AccountId { get; set; }



        /// <summary>
        ///     MMS API Key.
        /// </summary>

        public required string ApiKey { get; set; }



        /// <summary>
        ///     Gets or Sets BasicAuth
        /// </summary>
        public BasicAuthCredential BasicAuth { get; set; } = null!;
    }
}
