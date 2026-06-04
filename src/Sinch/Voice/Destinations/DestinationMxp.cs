using System.Text.Json.Serialization;

namespace Sinch.Voice.Destinations
{
    /// <summary>A data (app or web) endpoint identified by a username.</summary>
    public sealed class DestinationMxp : ICalloutDestination, ISinchEventDestination
    {
        /// <inheritdoc />
        [JsonPropertyName("type")]
        public DestinationType Type => DestinationType.Username;

        /// <inheritdoc />
        [JsonPropertyName("endpoint")]
        public required string Endpoint { get; set; }
    }
}
