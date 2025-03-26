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
            const string conversationApiUrlTemplate = "https://{0}.conversation.api.sinch.com/";
            return new Uri(ConversationUrlOverride ??
                           string.Format(conversationApiUrlTemplate,
                               ConversationRegion.Value));
        }

        internal Uri ResolveTemplateUrl()
        {
            const string templatesApiUrlTemplate = "https://{0}.template.api.sinch.com/";
            return new Uri(TemplateUrlOverride ??
                           string.Format(templatesApiUrlTemplate,
                               ConversationRegion.Value));
        }
    }
}
