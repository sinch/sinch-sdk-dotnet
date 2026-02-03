using System.Text.Json.Serialization;
using Sinch.Core;

namespace Sinch.SMS.Inbounds
{
    /// <summary>
    ///     Represents the SMS type options.
    /// </summary>
    [JsonConverter(typeof(EnumRecordJsonConverter<InboundMessageType>))]
    public record InboundMessageType(string Value) : EnumRecord(Value)
    {
        /// <summary>
        ///     Represents a text SMS type.
        /// </summary>
        public static readonly InboundMessageType Text = new("mo_text");

        /// <summary>
        ///     Represents a binary SMS type.
        /// </summary>
        public static readonly InboundMessageType Binary = new("mo_binary");

        /// <summary>
        ///     Represents an MMS type.
        /// </summary>
        public static readonly InboundMessageType Media = new("mo_media");

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
