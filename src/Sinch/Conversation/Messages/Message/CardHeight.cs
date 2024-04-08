using System.Text.Json.Serialization;
using Sinch.Core;

namespace Sinch.Conversation.Messages.Message
{
    /// <summary>
    ///     Represents the card height options.
    /// </summary>
    [JsonConverter(typeof(EnumRecordJsonConverter<CardHeight>))]
    public record CardHeight(string Value) : EnumRecord(Value)
    {
        /// <summary>
        ///     Unspecified height.
        /// </summary>
        public static readonly CardHeight UnspecifiedHeight = new("UNSPECIFIED_HEIGHT");

        /// <summary>
        ///     Short height.
        /// </summary>
        public static readonly CardHeight Short = new("SHORT");

        /// <summary>
        ///     Medium height.
        /// </summary>
        public static readonly CardHeight Medium = new("MEDIUM");

        /// <summary>
        ///     Tall height.
        /// </summary>
        public static readonly CardHeight Tall = new("TALL");
    }
}
