using System.Text.Json.Serialization;
using Sinch.Core;

namespace Sinch.Conversation.Messages.Message
{
    // [JsonDerivedType(typeof(TextMessage), typeDiscriminator: "text_message")]
    // [JsonInterfaceConverter(typeof(InterfaceConverter<IMessage>))]
    [JsonPolymorphic()]
    public interface IMessage
    {
    }
}
