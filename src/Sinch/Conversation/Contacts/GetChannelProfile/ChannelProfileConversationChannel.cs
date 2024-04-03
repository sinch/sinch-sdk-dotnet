using System.Text.Json.Serialization;
using Sinch.Core;

namespace Sinch.Conversation.Contacts.GetChannelProfile
{
    /// <summary>
    ///     The list of the channel which support channel profiles
    /// </summary>
    /// <param name="Value"></param>
    [JsonConverter(typeof(EnumRecordJsonConverter<ChannelProfileConversationChannel>))]
    public record ChannelProfileConversationChannel(string Value) : EnumRecord(Value)
    {
        public static readonly ChannelProfileConversationChannel Messenger = new(ConversationChannel.Messenger.Value);
        public static readonly ChannelProfileConversationChannel Instagram = new(ConversationChannel.Instagram.Value);
        public static readonly ChannelProfileConversationChannel Viber = new(ConversationChannel.Viber.Value);
        public static readonly ChannelProfileConversationChannel Line = new(ConversationChannel.Line.Value);
    }
}
