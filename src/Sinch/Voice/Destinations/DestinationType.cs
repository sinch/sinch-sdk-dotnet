using System.Text.Json.Serialization;
using Sinch.Core;

namespace Sinch.Voice.Destinations
{
    /// <summary>
    ///     All known Voice destination types across callouts, Sinch events, and streaming actions.
    /// </summary>
    [JsonConverter(typeof(EnumRecordCaseInsensitiveJsonConverter<DestinationType>))]
    public record DestinationType(string Value) : EnumRecord(Value)
    {
        /// <summary>A PSTN endpoint identified by an E.164 phone number.</summary>
        public static readonly DestinationType Number = new("number");
        /// <summary>A data (app or web) endpoint identified by a username.</summary>
        public static readonly DestinationType Username = new("username");
        /// <summary>A SIP endpoint identified by a SIP address.</summary>
        public static readonly DestinationType Sip = new("sip");
        /// <summary>A Direct Inward Dialling number — the number the caller actually dialled.</summary>
        public static readonly DestinationType Did = new("did");
        /// <summary>A WebSocket stream endpoint.</summary>
        public static readonly DestinationType Websocket = new("Websocket");
    }
}
