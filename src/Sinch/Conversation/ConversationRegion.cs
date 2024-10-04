namespace Sinch.Conversation
{
    /// <summary>
    ///     Represents the Conversation region options.
    /// </summary>
    public record ConversationRegion(string Value)
    {
        /// <summary>
        ///     US Production
        /// </summary>
        public static readonly ConversationRegion Us = new("us");

        /// <summary>
        ///     EU Production
        /// </summary>
        public static readonly ConversationRegion Eu = new("eu");

        /// <summary>
        ///     BR Production
        /// </summary>
        public static readonly ConversationRegion Br = new("br");
    }
}
