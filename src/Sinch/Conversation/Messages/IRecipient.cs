using System.Text.Json.Serialization;

namespace Sinch.Conversation.Messages
{
    [JsonDerivedType(typeof(Contact))]
    [JsonDerivedType(typeof(Identified))]
    public interface IRecipient
    {
    }
}
