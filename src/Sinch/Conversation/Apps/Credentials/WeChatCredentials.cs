using System.Text;

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
        public string AppId { get; set; }
        #endif
        

        /// <summary>
        ///     The AppSecret(Developer Password) for the WeChat channel to which you are connecting.
        /// </summary>
        #if NET7_0_OR_GREATER
        public required string AppSecret { get; set; }
        #else
        public string AppSecret { get; set; }
        #endif
        

        /// <summary>
        ///     The Token for the WeChat channel to which you are connecting.
        /// </summary>
        #if NET7_0_OR_GREATER
        public required string Token { get; set; }
        #else
        public string Token { get; set; }
        #endif
        

        /// <summary>
        ///     The Encoding AES Key for the WeChat channel to which you are connecting.
        /// </summary>
        #if NET7_0_OR_GREATER
        public required string AesKey { get; set; }
        #else
        public string AesKey { get; set; }
        #endif
        

        /// <summary>
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class WeChatCredentials {\n");
            sb.Append("  AppId: ").Append(AppId).Append("\n");
            sb.Append("  AppSecret: ").Append(AppSecret).Append("\n");
            sb.Append("  Token: ").Append(Token).Append("\n");
            sb.Append("  AesKey: ").Append(AesKey).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }

    }
}
