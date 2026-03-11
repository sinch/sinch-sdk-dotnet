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
    }
}
