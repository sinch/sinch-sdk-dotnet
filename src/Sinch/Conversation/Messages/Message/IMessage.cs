using Sinch.Core;

namespace Sinch.Conversation.Messages.Message
{
    /// <summary>
    ///     Marker interface for conversation messages types.
    /// </summary>
    [JsonInterfaceConverter(typeof(InterfaceConverter<IMessage>))]
    public interface IMessage
    {
    }
}
