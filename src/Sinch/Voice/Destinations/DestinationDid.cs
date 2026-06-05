using System.Text.Json.Serialization;

namespace Sinch.Voice.Destinations
{
    /// <summary>A Direct Inward Dialling number — the number the caller actually dialled.</summary>
    public sealed class DestinationDid : ISinchEventDestination
    {
        /// <inheritdoc />
        [JsonPropertyName("type")]
        public DestinationType Type => DestinationType.Did;

        /// <inheritdoc />
        [JsonPropertyName("endpoint")]
        public required string Endpoint { get; set; }
    }
}
