using System.Text;
using Sinch.Conversation.Apps.Credentials;

namespace Sinch.Conversation.Apps
{
    /// <summary>
    ///     Enables access to the underlying messaging channel.
    /// </summary>
    public sealed class ConversationChannelCredential
    {
        /// <summary>
        ///     Gets or Sets Channel
        /// </summary>
#if NET7_0_OR_GREATER
        public required ConversationChannel Channel { get; set; }
#else
        public ConversationChannel Channel { get; set; } = null!;
#endif

        /// <summary>
        ///     The secret used to verify the channel callbacks for channels which support callback verification.
        ///     The callback verification is not needed for Sinch-managed channels because
        ///     the callbacks are not leaving Sinch internal networks. Max length is 256 characters.
        ///     Note: leaving channel_callback_secret empty for channels
        ///     with callback verification will disable the verification.
        /// </summary>
        public string? CallbackSecret { get; set; }


        /// <summary>
        ///     Gets or Sets MmsCredentials
        /// </summary>
        public MmsCredentials? MmsCredentials { get; set; }


        /// <summary>
        ///     Gets or Sets KakaotalkCredentials
        /// </summary>
        public KakaoTalkCredentials? KakaotalkCredentials { get; set; }


        /// <summary>
        ///     Gets or Sets StaticBearer
        /// </summary>
        public StaticBearerCredential? StaticBearer { get; set; }


        /// <summary>
        ///     Gets or Sets StaticToken
        /// </summary>
        public StaticTokenCredential? StaticToken { get; set; }


        /// <summary>
        ///     Gets or Sets TelegramCredentials
        /// </summary>
        public TelegramCredentials? TelegramCredentials { get; set; }


        /// <summary>
        ///     Gets or Sets LineCredentials
        /// </summary>
        public LineCredentials? LineCredentials { get; set; }


        /// <summary>
        ///     Gets or Sets WechatCredentials
        /// </summary>
        public WeChatCredentials? WechatCredentials { get; set; }


        /// <summary>
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class ConversationChannelCredential {\n");
            sb.Append("  CallbackSecret: ").Append(CallbackSecret).Append("\n");
            sb.Append("  Channel: ").Append(Channel).Append("\n");
            sb.Append("  MmsCredentials: ").Append(MmsCredentials).Append("\n");
            sb.Append("  KakaoTalkCredentials: ").Append(KakaotalkCredentials).Append("\n");
            sb.Append("  StaticBearer: ").Append(StaticBearer).Append("\n");
            sb.Append("  StaticToken: ").Append(StaticToken).Append("\n");
            sb.Append("  TelegramCredentials: ").Append(TelegramCredentials).Append("\n");
            sb.Append("  LineCredentials: ").Append(LineCredentials).Append("\n");
            sb.Append("  WechatCredentials: ").Append(WechatCredentials).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }
    }
}
