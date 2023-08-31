using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using Sinch.Core;

namespace Sinch.Conversation.Messages.Message
{
    /// <summary>
    /// Defines ConversationDirection
    /// </summary>
    [JsonConverter(typeof(SinchEnumConverter<ConversationDirection>))]
    public enum ConversationDirection
    {
        /// <summary>
        /// Enum UNDEFINEDDIRECTION for value: UNDEFINED_DIRECTION
        /// </summary>
        [EnumMember(Value = "UNDEFINED_DIRECTION")]
        UndefinedDirection = 1,

        /// <summary>
        /// Enum TOAPP for value: TO_APP
        /// </summary>
        [EnumMember(Value = "TO_APP")]
        ToApp = 2,

        /// <summary>
        /// Enum TOCONTACT for value: TO_CONTACT
        /// </summary>
        [EnumMember(Value = "TO_CONTACT")]
        ToContact = 3
    }
}
