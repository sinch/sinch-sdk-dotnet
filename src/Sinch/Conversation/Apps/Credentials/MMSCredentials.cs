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
#if NET7_0_OR_GREATER
        public required string AccountId { get; set; }
#else
        public string AccountId { get; set; }
#endif


        /// <summary>
        ///     MMS API Key.
        /// </summary>
#if NET7_0_OR_GREATER
        public required string ApiKey { get; set; }
#else
        public string ApiKey { get; set; }
#endif


        /// <summary>
        ///     Gets or Sets BasicAuth
        /// </summary>
        public BasicAuthCredential BasicAuth { get; set; }
    }
}
