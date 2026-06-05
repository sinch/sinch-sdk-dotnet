using System.Text.Json.Serialization;

namespace Sinch.Voice.Destinations
{
    /// <summary>A SIP endpoint identified by a SIP address.</summary>
    public sealed class DestinationSip : ICalloutDestination, ISinchEventDestination
    {
        /// <inheritdoc />
        [JsonPropertyName("type")]
        public DestinationType Type => DestinationType.Sip;

        /// <inheritdoc />
        [JsonPropertyName("endpoint")]
        public required string Endpoint { get; set; }
    }
}
