using System.Text.Json.Serialization;

namespace Sinch.Voice.Destinations
{
    /// <summary>A PSTN endpoint identified by an E.164 phone number.</summary>
    public sealed class DestinationPstn : ICalloutDestination, ISinchEventDestination
    {
        /// <inheritdoc />
        [JsonPropertyName("type")]
        public DestinationType Type => DestinationType.Number;

        /// <inheritdoc />
        [JsonPropertyName("endpoint")]
        public required string Endpoint { get; set; }
    }
}
