using Sinch.Core;

namespace Sinch.Conversation.Conversations.List
{
    /// <summary>
    ///     Order of the recent conversations listing.
    /// </summary>
    public record ConversationOrder(string Value) : EnumRecord(Value)
    {
        /// <summary>
        ///     Descending order. Newest first.
        /// </summary>
        public static readonly ConversationOrder Desc = new("DESC");

        /// <summary>
        ///     Ascending order. Oldest first.
        /// </summary>
        public static readonly ConversationOrder Asc = new("ASC");
    }
}
