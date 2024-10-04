using System;
using Sinch.Conversation;
using Sinch.Voice;

namespace Sinch.Core
{
    internal static class UrlResolver
    {
        private const string ConversationApiUrlTemplate = "https://{0}.conversation.api.sinch.com/";

        public static Uri ResolveConversationUrl(ConversationRegion conversationRegion,
            ApiUrlOverrides? apiUrlOverrides)
        {
            return new Uri(apiUrlOverrides?.ConversationUrl ??
                           string.Format(ConversationApiUrlTemplate,
                               conversationRegion.Value));
        }

        private const string TemplatesApiUrlTemplate = "https://{0}.template.api.sinch.com/";

        public static Uri ResolveTemplateUrl(ConversationRegion conversationRegion,
            ApiUrlOverrides? apiUrlOverrides)
        {
            return new Uri(apiUrlOverrides?.TemplatesUrl ??
                           string.Format(TemplatesApiUrlTemplate,
                               conversationRegion.Value));
        }

        private const string AuthApiUrl = "https://auth.sinch.com";

        public static Uri ResolveAuthApiUrl(ApiUrlOverrides? apiUrlOverrides)
        {
            return new Uri(apiUrlOverrides?.AuthUrl ?? AuthApiUrl);
        }

        private const string VoiceApiUrlTemplate = "https://{0}.api.sinch.com/";

        public static Uri ResolveVoiceApiUrl(VoiceRegion voiceRegion, ApiUrlOverrides? apiUrlOverrides)
        {
            return new Uri(apiUrlOverrides?.VoiceUrl ?? string.Format(VoiceApiUrlTemplate, voiceRegion.Value));
        }

        // apparently, management api for applications have a different set url
        private const string VoiceApiApplicationManagementUrl = "https://callingapi.sinch.com/";

        public static Uri ResolveVoiceApiApplicationManagementUrl(ApiUrlOverrides? apiUrlOverrides)
        {
            return new Uri(apiUrlOverrides?.VoiceApplicationManagementUrl ?? VoiceApiApplicationManagementUrl);
        }
    }
}
