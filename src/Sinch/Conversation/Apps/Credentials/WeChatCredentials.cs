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
#if NET7_0_OR_GREATER
        public required string AppId { get; set; }
#else
        public string AppId { get; set; } = null!;
#endif


        /// <summary>
        ///     The AppSecret(Developer Password) for the WeChat channel to which you are connecting.
        /// </summary>
#if NET7_0_OR_GREATER
        public required string AppSecret { get; set; }
#else
        public string AppSecret { get; set; } = null!;
#endif


        /// <summary>
        ///     The Token for the WeChat channel to which you are connecting.
        /// </summary>
#if NET7_0_OR_GREATER
        public required string Token { get; set; }
#else
        public string Token { get; set; } = null!;
#endif


        /// <summary>
        ///     The Encoding AES Key for the WeChat channel to which you are connecting.
        /// </summary>
#if NET7_0_OR_GREATER
        public required string AesKey { get; set; }
#else
        public string AesKey { get; set; } = null!;
#endif
    }
}
