using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using Sinch.Core;

namespace Sinch.Voice.Calls.Actions
{
    /// <summary>
    ///     Determines how to route a call to a SIP server. 
    /// </summary>
    public sealed class ConnectSip : IAction
    {
        /// <summary>
        ///     Gets or Sets Destination
        /// </summary>
        [JsonPropertyName("destination")]
#if NET7_0_OR_GREATER
        public required ConnectSipDestination Destination { get; set; }
#else
        public ConnectSipDestination Destination { get; set; }
#endif


        /// <summary>
        ///     The max duration of the call in seconds (max 14400 seconds). If the call is still connected at that time, it will
        ///     be automatically disconnected.
        /// </summary>
        [JsonPropertyName("maxDuration")]
        public int? MaxDuration { get; set; }


        /// <summary>
        ///     Used to override the CLI (or caller ID) of the client. The phone number of the person who initiated the call is
        ///     shown as the CLI. To set your own CLI, you may use your verified number or your Dashboard virtual number.
        /// </summary>
        [JsonPropertyName("cli")]
        public string Cli { get; set; }


        /// <summary>
        ///     An optional parameter to specify the SIP transport protocol. If unspecified, UDP is used.
        /// </summary>
        [JsonPropertyName("transport")]
        public Transport Transport { get; set; }


        /// <summary>
        ///     If enabled, suppresses
        ///     <see
        ///         href="https://developers.sinch.com/docs/voice/api-reference/voice/voice/tag/Callbacks/#tag/Callbacks/operation/ace">
        ///         ACE
        ///     </see>
        ///     and
        ///     <see
        ///         href="https://developers.sinch.com/docs/voice/api-reference/voice/voice/tag/Callbacks/#tag/Callbacks/operation/dice">
        ///         DICE
        ///     </see>
        ///     callbacks for the call.
        /// </summary>
        [JsonPropertyName("suppressCallbacks")]
        public bool? SuppressCallbacks { get; set; }


        /// <summary>
        ///     <see
        ///         href="https://developers.sinch.com/docs/voice/api-reference/voice/sip-trunking/#receiving-calls-from-sinch-platform-to-your-sip-infrastructure">
        ///         Private
        ///         SIP headers
        ///     </see>
        ///     to send with the call.
        /// </summary>
        [JsonPropertyName("callHeaders")]
        public List<CallHeader> CallHeaders { get; set; }


        /// <summary>
        ///     Means \&quot;music on hold\&quot;. If this optional parameter is included, plays music to the connected participant
        ///     if the SIP call is placed on hold. If &#x60;moh&#x60; isn&#39;t specified and the SIP call is placed on hold, the
        ///     user will only hear silence while during the holding period .
        /// </summary>
        [JsonPropertyName("moh")]
        public MohClass Moh { get; set; }

        public string Name { get; } = "connectSip";


        /// <summary>
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class ConnectSip {\n");
            sb.Append("  Name: ").Append(Name).Append("\n");
            sb.Append("  Destination: ").Append(Destination).Append("\n");
            sb.Append("  MaxDuration: ").Append(MaxDuration).Append("\n");
            sb.Append("  Cli: ").Append(Cli).Append("\n");
            sb.Append("  Transport: ").Append(Transport).Append("\n");
            sb.Append("  SuppressCallbacks: ").Append(SuppressCallbacks).Append("\n");
            sb.Append("  CallHeaders: ").Append(CallHeaders).Append("\n");
            sb.Append("  Moh: ").Append(Moh).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }
    }

    [JsonConverter(typeof(EnumRecordJsonConverter<Transport>))]
    public record Transport(string Value) : EnumRecord(Value)
    {
        public static readonly Transport UDP = new("UDP");
        public static readonly Transport TCP = new("TCP");
        public static readonly Transport TLS = new("TLS");
    }

    /// <summary>
    ///     Specifies where to route the SIP call.
    /// </summary>
    public sealed class ConnectSipDestination
    {
        /// <summary>
        ///     The SIP address.
        /// </summary>
        [JsonPropertyName("endpoint")]
#if NET7_0_OR_GREATER
        public required string Endpoint { get; set; }
#else
        public string Endpoint { get; set; }
#endif


        /// <summary>
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class SvamlActionConnectSipDestination {\n");
            sb.Append("  Endpoint: ").Append(Endpoint).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }
    }
}
