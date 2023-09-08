using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using Sinch.Core;

namespace Sinch.Conversation.Apps
{
    /// <summary>
    ///     Whether or not Conversation API should store contacts and conversations for the app.
    ///     For more information,
    ///     see <see href="https://developers.sinch.com/docs/conversation/processing-modes/">Processing Modes</see>.
    /// </summary>
    [JsonConverter(typeof(SinchEnumConverter<ProcessingMode>))]
    public enum ProcessingMode 
    {
        /// <summary>
        ///     The default Processing Mode. Creates contacts and conversations automatically
        ///     when a message is sent or received and there's no existing contact or active conversation.
        /// </summary>
        [EnumMember(Value = "CONVERSATION")]
        Conversation,
        /// <summary>
        ///     Does not associate messages with contacts and conversations.
        ///     This processing mode is mostly intended for unidirectional high volume SMS use cases.
        ///     The lack of contacts and conversations limits some API features as related data won't be
        ///     queryable in the
        ///     <see href="https://developers.sinch.com/docs/conversation/api-reference/conversation/tag/Contact">
        ///     Contact
        ///     </see> and
        ///     <see href="https://developers.sinch.com/docs/conversation/api-reference/conversation/tag/Conversation">
        ///     Conversation</see> APIs.
        /// </summary>
        [EnumMember(Value = "DISPATCH")]
        Dispatch
    }
}
