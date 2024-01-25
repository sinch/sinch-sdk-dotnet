using System.Text.Json.Serialization;

namespace Sinch.Conversation.Messages
{
    [JsonDerivedType(typeof(ContactRecipient))]
    [JsonDerivedType(typeof(Identified))]
    public interface IRecipient
    {
    }
}
