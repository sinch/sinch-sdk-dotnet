using System;

namespace Sinch.Conversation
{
    public sealed class SinchConversationConfiguration
    {
        /// <summary>
        ///     Sets the region for the Conversation API.
        ///     Required. See <see cref="ConversationRegion" /> for available values.
        /// </summary>
        public ConversationRegion? ConversationRegion { get; init; }

        public string? ConversationUrlOverride { get; init; }

        public string? TemplateUrlOverride { get; init; }


        internal Uri ResolveConversationUrl()
        {
            const string conversationApiUrlTemplate = "https://{0}.conversation.api.sinch.com/";
            return new Uri(ConversationUrlOverride ??
                           string.Format(conversationApiUrlTemplate, ConversationRegion!.Value));
        }

        internal Uri ResolveTemplateUrl()
        {
            const string templatesApiUrlTemplate = "https://{0}.template.api.sinch.com/";
            return new Uri(TemplateUrlOverride ??
                           string.Format(templatesApiUrlTemplate, ConversationRegion!.Value));
        }

        internal void Validate()
        {
            if (ConversationRegion == null)
            {
                throw new InvalidOperationException(
                    $"{nameof(SinchConversationConfiguration)}.{nameof(ConversationRegion)} is required. " +
                    $"Set it to one of the values in {nameof(ConversationRegion)}, e.g. {nameof(ConversationRegion)}.{nameof(Sinch.Conversation.ConversationRegion.Us)}.");
            }
        }
    }
}
