namespace Sinch.Conversation.Apps.Credentials
{
    /// <summary>
    ///     If you are including the WeChat channel in the &#x60;channel_identifier&#x60; property, you must include this object.
    /// </summary>
    public sealed class WeChatCredentials
    {
        /// <summary>
        ///     The AppID(Developer ID) for the WeChat channel to which you are connecting.
        /// </summary>

        public required string AppId { get; set; }



        /// <summary>
        ///     The AppSecret(Developer Password) for the WeChat channel to which you are connecting.
        /// </summary>

        public required string AppSecret { get; set; }



        /// <summary>
        ///     The Token for the WeChat channel to which you are connecting.
        /// </summary>

        public required string Token { get; set; }



        /// <summary>
        ///     The Encoding AES Key for the WeChat channel to which you are connecting.
        /// </summary>

        public required string AesKey { get; set; }

    }
}
