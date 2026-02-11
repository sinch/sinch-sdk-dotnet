namespace Sinch.Conversation.Apps.Credentials
{
    /// <summary>
    ///     Factory helper for creating channel-specific credentials with correct channel mapping.
    /// </summary>
    public static class ConversationChannelCredentialsBuilderFactory
    {
        public static ConversationChannelCredentials AppleBc(AppleBusinessChatCredentials credentials) =>
            new(credentials)
            {
                Channel = ConversationChannel.AppleBC
            };

        public static ConversationChannelCredentials Instagram(InstagramCredentials credentials) =>
            new(credentials)
            {
                Channel = ConversationChannel.Instagram
            };

        public static ConversationChannelCredentials KakaoTalk(KakaoTalkCredentials credentials) =>
            new(credentials)
            {
                Channel = ConversationChannel.KakaoTalk
            };

        public static ConversationChannelCredentials KakaoTalkChat(KakaoTalkChatCredentials credentials) =>
            new(credentials)
            {
                Channel = ConversationChannel.KakaoTalkChat
            };

        public static ConversationChannelCredentials Line(LineCredentials credentials) =>
            new(credentials)
            {
                Channel = ConversationChannel.Line
            };

        public static ConversationChannelCredentials LineEnterprise(LineJapanEnterpriseCredentials credentials) =>
            new(credentials)
            {
                Channel = ConversationChannel.Line
            };

        public static ConversationChannelCredentials LineEnterprise(LineThailandEnterpriseCredentials credentials) =>
            new(credentials)
            {
                Channel = ConversationChannel.Line
            };

        public static ConversationChannelCredentials Messenger(StaticTokenCredentials credentials) =>
            new(credentials)
            {
                Channel = ConversationChannel.Messenger
            };

        public static ConversationChannelCredentials Mms(MmsCredentials credentials) =>
            new(credentials)
            {
                Channel = ConversationChannel.Mms
            };

        public static ConversationChannelCredentials Rcs(StaticBearerCredentials credentials) =>
            new(credentials)
            {
                Channel = ConversationChannel.Rcs
            };

        public static ConversationChannelCredentials Sms(StaticBearerCredentials credentials) =>
            new(credentials)
            {
                Channel = ConversationChannel.Sms
            };

        public static ConversationChannelCredentials Telegram(TelegramCredentials credentials) =>
            new(credentials)
            {
                Channel = ConversationChannel.Telegram
            };

        public static ConversationChannelCredentials Viber(StaticTokenCredentials credentials) =>
            new(credentials)
            {
                Channel = ConversationChannel.Viber
            };

        public static ConversationChannelCredentials ViberBm(StaticBearerCredentials credentials) =>
            new(credentials)
            {
                Channel = ConversationChannel.ViberBm
            };

        public static ConversationChannelCredentials WeChat(WeChatCredentials credentials) =>
            new(credentials)
            {
                Channel = ConversationChannel.WeChat
            };

        public static ConversationChannelCredentials WhatsApp(StaticBearerCredentials credentials) =>
            new(credentials)
            {
                Channel = ConversationChannel.WhatsApp
            };
    }
}
