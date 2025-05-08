using System.Text.Json.Serialization;
using Sinch.Core;

namespace Sinch.Voice.Common
{
    /// <summary>
    ///     Known destination.
    /// </summary>
    [JsonConverter(typeof(EnumRecordCaseInsensitiveJsonConverter<ParticipantType>))]
    public record ParticipantType(string Value) : EnumRecord(Value)
    {
        /// <summary>
        ///     Destination ptsn
        /// </summary>
        public static readonly ParticipantType Number = new("number");
        /// <summary>
        ///     Destination mxp
        /// </summary>
        public static readonly ParticipantType Username = new("username");
        public static readonly ParticipantType Sip = new("sip");
    }
}
