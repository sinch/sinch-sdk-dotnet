using System;
using Sinch.Conversation;

namespace Sinch.Core
{
    internal static class UrlResolver
    {
        private const string ConversationApiUrlTemplate = "https://{0}.conversation.api.sinch.com/";
        private const string TemplatesApiUrlTemplate = "https://{0}.template.api.sinch.com/";

        public static Uri ResolveConversationUrl(ConversationRegion conversationRegion,
            ApiUrlOverrides? apiUrlOverrides)
        {
            return new Uri(apiUrlOverrides?.ConversationUrl ??
                           string.Format(ConversationApiUrlTemplate,
                               conversationRegion.Value));
        }

        public static Uri ResolveTemplateUrl(ConversationRegion conversationRegion,
            ApiUrlOverrides? apiUrlOverrides)
        {
            return new Uri(apiUrlOverrides?.TemplatesUrl ??
                           string.Format(TemplatesApiUrlTemplate,
                               conversationRegion.Value));
        }
    }
}
