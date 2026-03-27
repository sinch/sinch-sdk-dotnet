using System.Text.Json.Serialization;

namespace Sinch.Conversation.Contacts.Merge;

public class MergeContactRequest
{
    /// <summary>
    ///     The ID of the contact to be removed and merged into the destination contact.
    /// </summary>
    [JsonPropertyName("source_id")]
    public string? SourceId { get; set; }

    /// <summary>
    ///     Strategy to use when merging the two contacts.
    ///     When not set, the backend default applies.
    /// </summary>
    [JsonPropertyName("strategy")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ContactMergeStrategy? Strategy { get; set; }
}
