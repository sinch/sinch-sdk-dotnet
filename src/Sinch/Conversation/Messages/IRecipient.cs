using Sinch.Core;

namespace Sinch.Conversation.Messages
{
    [JsonInterfaceConverter(typeof(InterfaceConverter<IRecipient>))]
    public interface IRecipient
    {
    }
}
