using System.Text.Json.Serialization;

namespace Sinch.Voice.Destinations
{
    /// <summary>A WebSocket stream endpoint used with the <c>connectStream</c> SVAML action.</summary>
    public sealed class DestinationWebSocket : IStreamDestination
    {
        /// <inheritdoc />
        [JsonPropertyName("type")]
        public DestinationType Type => DestinationType.Websocket;

        /// <inheritdoc />
        [JsonPropertyName("endpoint")]
        public required string Endpoint { get; set; }
    }
}
