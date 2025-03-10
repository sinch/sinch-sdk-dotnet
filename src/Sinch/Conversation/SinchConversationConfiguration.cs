using System;

namespace Sinch.Conversation
{
    public sealed class SinchConversationConfiguration
    {
        public ConversationRegion ConversationRegion { get; init; } = ConversationRegion.Us;

        public string? ConversationUrlOverride { get; init; }

        public string? TemplateUrlOverride { get; init; }


        internal Uri ResolveConversationUrl()
        {
            if (!string.IsNullOrEmpty(ConversationUrlOverride))
            {
                return new Uri(ConversationUrlOverride);
            }

            const string conversationApiUrlTemplate = "https://{0}.conversation.api.sinch.com/";
            return new Uri(
                string.Format(conversationApiUrlTemplate,
                    ConversationRegion.Value));
        }

        internal Uri ResolveTemplateUrl()
        {
            if (!string.IsNullOrEmpty(TemplateUrlOverride))
            {
                return new Uri(TemplateUrlOverride);
            }

            const string templatesApiUrlTemplate = "https://{0}.template.api.sinch.com/";
            return new Uri(
                string.Format(templatesApiUrlTemplate,
                    ConversationRegion.Value));
        }
    }
}
