namespace Sinch.Conversation.Apps.Credentials
{
    /// <summary>
    ///     If you are including the LINE channel in the &#x60;channel_identifier&#x60; property, you must include this object.
    /// </summary>
    public sealed class LineCredentials
    {
        /// <summary>
        ///     The token for the LINE channel to which you are connecting.
        /// </summary>
#if NET7_0_OR_GREATER
        public required string Token { get; set; }
#else
        public string Token { get; set; }
#endif


        /// <summary>
        ///     The secret for the LINE channel to which you are connecting.
        /// </summary>
#if NET7_0_OR_GREATER
        public required string Secret { get; set; }
#else
        public string Secret { get; set; }
#endif
    }
}
