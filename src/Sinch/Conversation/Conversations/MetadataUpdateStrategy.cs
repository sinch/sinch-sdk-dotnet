using Sinch.Core;

namespace Sinch.Conversation.Conversations
{
    /// <summary>
    ///     Update strategy for the conversation_metadata field.
    /// </summary>
    /// <param name="Value"></param>
    public record MetadataUpdateStrategy(string Value) : EnumRecord(Value)
    {
        /// <summary>
        ///     The default strategy. Replaces the whole conversation_metadata field with the new value provided.
        /// </summary>
        public static readonly MetadataUpdateStrategy Replace = new("REPLACE");

        /// <summary>
        ///     Patches the conversation_metadata field with the patch provided according to RFC 7386.
        /// </summary>
        public static readonly MetadataUpdateStrategy MergePatch = new("MERGE_PATCH");
    }
}
