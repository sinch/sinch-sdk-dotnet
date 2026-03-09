using System;

namespace Sinch.Conversation
{
    public sealed class SinchConversationConfiguration
    {
        /// <summary>
        ///     Sets the region for the Conversation API.
        ///     Required. See <see cref="Region" /> for available values.
        /// </summary>
        public ConversationRegion? Region { get; init; }

        public string? ConversationUrlOverride { get; init; }

        public string? TemplateUrlOverride { get; init; }
        
        private static string ConversationRegionRequiredMessage =>
            $"{nameof(SinchConversationConfiguration)}.{nameof(Region)} is required. " +
            $"Set it to one of the values in {nameof(Region)}, e.g. {nameof(Region)}.{nameof(ConversationRegion.Us)}.";

        internal Uri ResolveUrl()
        {
            if (ConversationUrlOverride is not null)
                return new Uri(ConversationUrlOverride);

            if (Region is null)
                throw new InvalidOperationException(ConversationRegionRequiredMessage);

            return new Uri($"https://{Region.Value}.conversation.api.sinch.com/");
        }

        internal Uri ResolveTemplateUrl()
        {
            if (TemplateUrlOverride is not null)
                return new Uri(TemplateUrlOverride);

            if (Region is null)
                throw new InvalidOperationException(ConversationRegionRequiredMessage);

            return new Uri($"https://{Region.Value}.template.api.sinch.com/");
        }

        internal void Validate()
        {
            if (Region == null)
                throw new InvalidOperationException(ConversationRegionRequiredMessage);
        }
    }
}
