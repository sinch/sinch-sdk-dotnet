using Sinch.Core;

namespace Sinch.Conversation.Common
{
    [JsonInterfaceConverter(typeof(InterfaceConverter<IRecipient>))]
    public interface IRecipient
    {
    }
}
