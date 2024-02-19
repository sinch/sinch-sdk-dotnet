using System.Text.Json.Serialization;

namespace Sinch.Conversation.Common
{
    [JsonDerivedType(typeof(ContactRecipient))]
    [JsonDerivedType(typeof(Identified))]
    public interface IRecipient
    {
    }
}
