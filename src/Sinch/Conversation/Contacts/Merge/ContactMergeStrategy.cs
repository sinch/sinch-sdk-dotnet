using System.Text.Json.Serialization;
using Sinch.Core;

namespace Sinch.Conversation.Contacts.Merge;

[JsonConverter(typeof(EnumRecordJsonConverter<ContactMergeStrategy>))]
public record ContactMergeStrategy(string Value) : EnumRecord(Value)
{
    /// <summary>
    ///     Merge the source contact into the destination contact.
    /// </summary>
    public static readonly ContactMergeStrategy Merge = new("MERGE");
}
