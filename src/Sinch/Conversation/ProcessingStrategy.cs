using System.Text.Json.Serialization;
using Sinch.Core;

namespace Sinch.Conversation
{

    [JsonConverter(typeof(EnumRecordJsonConverter<ProcessingStrategy>))]
    public record ProcessingStrategy(string Value) : EnumRecord(Value)
    {
        /// <summary>
        ///     The request will inherit the app's configured processing mode.
        /// </summary>
        public static readonly ProcessingStrategy Default = new("DEFAULT");
        /// <summary>
        ///     Forces the request to be processed in dispatch mode (without storing contacts and conversations),
        ///     regardless of the app's configured processing mode. Please note that user replies will still
        ///     be processed in the app's default processing mode and that the `conversation_metadata`
        ///     and `correlation_id` fields are not supported when using this option with an app in `CONVERSATION` mode.
        /// </summary>
        public static readonly ProcessingStrategy DispatchOnly = new("DISPATCH_ONLY");
    }
}
