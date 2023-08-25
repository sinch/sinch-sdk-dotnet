using System.Text.Json.Serialization;
using Sinch.Core;

namespace Sinch.Conversation.Messages.Message
{
    /// <summary>
    ///     Marker interface for conversation messages types.
    /// </summary>
    [JsonDerivedType(typeof(CardMessage))]
    [JsonDerivedType(typeof(CarouselMessage))]
    [JsonDerivedType(typeof(ChoiceMessage))]
    [JsonDerivedType(typeof(ListMessage))]
    [JsonDerivedType(typeof(LocationMessage))]
    [JsonDerivedType(typeof(MediaMessage))]
    [JsonDerivedType(typeof(TemplateMessage))]
    [JsonDerivedType(typeof(TextMessage))]
    [JsonInterfaceConverter(typeof(InterfaceConverter<MessageBase>))]
    public interface MessageBase
    {
    }
}
