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


        private static string ConversationRegionRequiredMessage =>
            $"{nameof(SinchConversationConfiguration)}.{nameof(ConversationRegion)} is required. " +
            $"Set it to one of the values in {nameof(ConversationRegion)}, e.g. {nameof(ConversationRegion)}.{nameof(Sinch.Conversation.ConversationRegion.Us)}.";

        internal Uri ResolveUrl()
        {
            if (ConversationUrlOverride is not null)
                return new Uri(ConversationUrlOverride);

            if (ConversationRegion is null)
                throw new InvalidOperationException(ConversationRegionRequiredMessage);

            return new Uri(string.Format("https://{0}.conversation.api.sinch.com/", ConversationRegion.Value));
        }

        internal Uri ResolveTemplateUrl()
        {
            if (TemplateUrlOverride is not null)
                return new Uri(TemplateUrlOverride);

            if (ConversationRegion is null)
                throw new InvalidOperationException(ConversationRegionRequiredMessage);

            return new Uri(string.Format("https://{0}.template.api.sinch.com/", ConversationRegion.Value));
        }

        internal void Validate()
        {
            if (ConversationRegion == null)
                throw new InvalidOperationException(ConversationRegionRequiredMessage);
        }
    }
}
