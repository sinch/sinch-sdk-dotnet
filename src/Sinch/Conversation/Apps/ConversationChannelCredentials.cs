using System;
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

        [JsonConstructor]
        [Obsolete("Needed for System.Text.Json", true)]
        public ConversationChannelCredentials()
        {
        }

        #region Oneof credential props and constructors

        [JsonInclude]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("static_bearer")]
        public StaticBearerCredentials? StaticBearer { get; private set; }

        [JsonInclude]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("static_token")]
        public StaticTokenCredentials? StaticToken { get; private set; }

        [JsonInclude]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("mms_credentials")]
        public MmsCredentials? MmsCredentials { get; private set; }

        [JsonInclude]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("kakaotalk_credentials")]
        public KakaoTalkCredentials? KakaoTalkCredentials { get; private set; }

        [JsonInclude]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("telegram_credentials")]
        public TelegramCredentials? TelegramCredentials { get; private set; }

        [JsonInclude]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("line_credentials")]
        public LineCredentials? LineCredentials { get; private set; }

        [JsonInclude]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("line_enterprise_credentials")]
        public LineEnterpriseCredentials? LineEnterpriseCredentials { get; private set; }

        [JsonInclude]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("wechat_credentials")]
        public WeChatCredentials? WechatCredentials { get; private set; }

        [JsonInclude]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("instagram_credentials")]
        public InstagramCredentials? InstagramCredentials { get; private set; }

        [JsonInclude]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("applebc_credentials")]
        public AppleBusinessChatCredentials? ApplebcCredentials { get; private set; }

        [JsonInclude]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("kakaotalkchat_credentials")]
        public KakaoTalkChatCredentials? KakaoTalkChatCredentials { get; private set; }

        public ConversationChannelCredentials(StaticBearerCredentials staticBearer)
        {
            StaticBearer = staticBearer;
        }

        public ConversationChannelCredentials(StaticTokenCredentials staticToken)
        {
            StaticToken = staticToken;
        }

        public ConversationChannelCredentials(MmsCredentials mmsCredentials)
        {
            MmsCredentials = mmsCredentials;
        }

        public ConversationChannelCredentials(KakaoTalkCredentials kakaoTalkCredentials)
        {
            KakaoTalkCredentials = kakaoTalkCredentials;
        }

        public ConversationChannelCredentials(TelegramCredentials telegramCredentials)
        {
            TelegramCredentials = telegramCredentials;
        }

        public ConversationChannelCredentials(LineCredentials lineCredentials)
        {
            LineCredentials = lineCredentials;
        }

        public ConversationChannelCredentials(LineEnterpriseCredentials lineEnterpriseCredentials)
        {
            LineEnterpriseCredentials = lineEnterpriseCredentials;
        }

        public ConversationChannelCredentials(WeChatCredentials wechatCredentials)
        {
            WechatCredentials = wechatCredentials;
        }

        public ConversationChannelCredentials(InstagramCredentials instagramCredentials)
        {
            InstagramCredentials = instagramCredentials;
        }

        public ConversationChannelCredentials(AppleBusinessChatCredentials applebcCredentials)
        {
            ApplebcCredentials = applebcCredentials;
        }

        public ConversationChannelCredentials(KakaoTalkChatCredentials kakaoTalkChatCredentials)
        {
            KakaoTalkChatCredentials = kakaoTalkChatCredentials;
        }

        #endregion


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
            sb.Append($"  {nameof(CallbackSecret)}: ").Append(Consts.HiddenString).Append('\n');
            sb.Append($"  {nameof(Channel)}: ").Append(Channel).Append('\n');
            sb.Append($"  {nameof(State)}: ").Append(State).Append('\n');
            sb.Append($"  {nameof(ChannelKnownId)}: ").Append(ChannelKnownId).Append('\n');
            sb.Append($"  {nameof(CredentialOrdinalNumber)}: ").Append(CredentialOrdinalNumber).Append('\n');
            sb.Append("}\n");
            return sb.ToString();
        }
    }
}
