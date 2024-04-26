using Sinch.Core;

namespace Sinch.Conversation.Messages.Message
{
    /// <summary>
    ///     Marker interface for app messages
    /// </summary>
    [JsonInterfaceConverter(typeof(InterfaceConverter<IMessage>))]
    public interface IMessage
    {
    }
}
