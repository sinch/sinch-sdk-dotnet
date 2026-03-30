using System.Collections.Generic;

namespace Sinch.Conversation.Contacts
{
    /// <summary>
    ///     Represents a conflict where the same channel identity is linked to multiple contacts.
    /// </summary>
    public sealed class IdentityConflict
    {
        /// <summary>
        ///     The channel identity value that is shared across multiple contacts.
        /// </summary>
        public string? Identity { get; set; }

        /// <summary>
        ///     The list of channels on which the identity conflict exists.
        /// </summary>
        public List<ConversationChannel>? Channels { get; set; }

        /// <summary>
        ///     The list of contact IDs that share the same channel identity.
        /// </summary>
        public List<string>? ContactIds { get; set; }
    }
}
