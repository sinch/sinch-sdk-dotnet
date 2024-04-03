using System.Text.Json.Serialization;
using Sinch.Core;

namespace Sinch.Conversation.Common
{
    /// <summary>
    ///     Represents the processing mode options for Conversation API.
    /// </summary>
    [JsonConverter(typeof(EnumRecordJsonConverter<ProcessingMode>))]
    public record ProcessingMode(string Value) : EnumRecord(Value)
    {
        /// <summary>
        ///     The default Processing Mode. Creates contacts and conversations automatically
        ///     when a message is sent or received and there's no existing contact or active conversation.
        /// </summary>
        public static readonly ProcessingMode Conversation = new("CONVERSATION");

        /// <summary>
        ///     Does not associate messages with contacts and conversations.
        ///     This processing mode is mostly intended for unidirectional high volume SMS use cases.
        ///     The lack of contacts and conversations limits some API features as related data won't be
        ///     queryable in the Contact and Conversation APIs.
        /// </summary>
        public static readonly ProcessingMode Dispatch = new("DISPATCH");
    }
}
