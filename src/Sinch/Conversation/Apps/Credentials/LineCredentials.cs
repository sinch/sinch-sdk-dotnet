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

        public required string Token { get; set; }



        /// <summary>
        ///     The secret for the LINE channel to which you are connecting.
        /// </summary>

        public required string Secret { get; set; }

    }
}
