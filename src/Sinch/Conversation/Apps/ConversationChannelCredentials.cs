using System.Text;
using System.Text.Json.Serialization;
using Sinch.Conversation.Apps.Credentials;

namespace Sinch.Conversation.Apps
{
    /// <summary>
    ///     Enables access to the underlying messaging channel.
    /// </summary>
    public sealed class ConversationChannelCredentials
    {
        /// <summary>
        /// Gets or Sets Channel
        /// </summary>
        [JsonPropertyName("channel")]
        public ConversationChannel? Channel { get; set; }

        /// <summary>
        ///     Gets or Sets StaticBearer
        /// </summary>
        [JsonPropertyName("static_bearer")]
        public StaticBearerCredentials? StaticBearer { get; set; }


        /// <summary>
        ///     Gets or Sets StaticToken
        /// </summary>
        [JsonPropertyName("static_token")]
        public StaticTokenCredentials? StaticToken { get; set; }


        /// <summary>
        ///     Gets or Sets MmsCredentials
        /// </summary>
        [JsonPropertyName("mms_credentials")]
        public MmsCredentials? MmsCredentials { get; set; }


        /// <summary>
        ///     Gets or Sets KakaotalkCredentials
        /// </summary>
        [JsonPropertyName("kakaotalk_credentials")]
        public KakaoTalkCredentials? KakaoTalkCredentials { get; set; }


        /// <summary>
        ///     Gets or Sets TelegramCredentials
        /// </summary>
        [JsonPropertyName("telegram_credentials")]
        public TelegramCredentials? TelegramCredentials { get; set; }


        /// <summary>
        ///     Gets or Sets LineCredentials
        /// </summary>
        [JsonPropertyName("line_credentials")]
        public LineCredentials? LineCredentials { get; set; }


        /// <summary>
        ///     Gets or Sets LineEnterpriseCredentials
        /// </summary>
        [JsonPropertyName("line_enterprise_credentials")]
        public LineEnterpriseCredentials? LineEnterpriseCredentials { get; set; }


        /// <summary>
        ///     Gets or Sets WechatCredentials
        /// </summary>
        [JsonPropertyName("wechat_credentials")]
        public WeChatCredentials? WechatCredentials { get; set; }


        /// <summary>
        ///     Gets or Sets InstagramCredentials
        /// </summary>
        [JsonPropertyName("instagram_credentials")]
        public InstagramCredentials? InstagramCredentials { get; set; }


        /// <summary>
        ///     Gets or Sets ApplebcCredentials
        /// </summary>
        [JsonPropertyName("applebc_credentials")]
        public AppleBusinessChatCredentials? ApplebcCredentials { get; set; }


        /// <summary>
        ///     Gets or Sets KakaotalkchatCredentials
        /// </summary>
        [JsonPropertyName("kakaotalkchat_credentials")]
        public KakaoTalkChatCredentials? KakaoTalkChatCredentials { get; set; }


        /// <summary>
        ///     The secret used to verify the channel callbacks for channels which support callback verification. The callback verification is not needed for Sinch-managed channels because the callbacks are not leaving Sinch internal networks. Max length is 256 characters. Note: leaving channel_callback_secret empty for channels with callback verification will disable the verification.
        /// </summary>
        [JsonPropertyName("callback_secret")]
        public string? CallbackSecret { get; set; }


        /// <summary>
        ///     Gets or Sets State
        /// </summary>
        [JsonPropertyName("state")]
        public ChannelIntegrationState? State { get; set; }


        /// <summary>
        ///     Additional identifier set by the channel that represents an specific id used by the channel.
        /// </summary>
        [JsonPropertyName("channel_known_id")]
        public string? ChannelKnownId { get; set; }


        /// <summary>
        ///     Gets or Sets CredentialOrdinalNumber
        /// </summary>
        [JsonPropertyName("credential_ordinal_number")]
        public int CredentialOrdinalNumber { get; set; }


        /// <summary>
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append($"class {nameof(ConversationChannelCredentials)} {{\n");
            sb.Append($"  {nameof(StaticBearer)}: ").Append(StaticBearer).Append('\n');
            sb.Append($"  {nameof(StaticToken)}: ").Append(StaticToken).Append('\n');
            sb.Append($"  {nameof(MmsCredentials)}: ").Append(MmsCredentials).Append('\n');
            sb.Append($"  {nameof(KakaoTalkCredentials)}: ").Append(KakaoTalkCredentials).Append('\n');
            sb.Append($"  {nameof(TelegramCredentials)}: ").Append(TelegramCredentials).Append('\n');
            sb.Append($"  {nameof(LineCredentials)}: ").Append(LineCredentials).Append('\n');
            sb.Append($"  {nameof(LineEnterpriseCredentials)}: ").Append(LineEnterpriseCredentials).Append('\n');
            sb.Append($"  {nameof(WechatCredentials)}: ").Append(WechatCredentials).Append('\n');
            sb.Append($"  {nameof(InstagramCredentials)}: ").Append(InstagramCredentials).Append('\n');
            sb.Append($"  {nameof(ApplebcCredentials)}: ").Append(ApplebcCredentials).Append('\n');
            sb.Append($"  {nameof(KakaoTalkChatCredentials)}: ").Append(KakaoTalkChatCredentials).Append('\n');
            sb.Append($"  {nameof(CallbackSecret)}: ").Append(CallbackSecret).Append('\n');
            sb.Append($"  {nameof(Channel)}: ").Append(Channel).Append('\n');
            sb.Append($"  {nameof(State)}: ").Append(State).Append('\n');
            sb.Append($"  {nameof(ChannelKnownId)}: ").Append(ChannelKnownId).Append('\n');
            sb.Append($"  {nameof(CredentialOrdinalNumber)}: ").Append(CredentialOrdinalNumber).Append('\n');
            sb.Append("}\n");
            return sb.ToString();
        }
    }
}
