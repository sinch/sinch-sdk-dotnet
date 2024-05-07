namespace Sinch.Conversation
{
    /// <summary>
    ///     Represents the Conversation region options.
    /// </summary>
    public record ConversationRegion(string Value)
    {
        public static readonly ConversationRegion Us = new("us");

        public static readonly ConversationRegion Eu = new("eu");
    }
}
