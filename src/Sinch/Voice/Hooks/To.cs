using System.Text.Json.Serialization;

namespace Sinch.Voice.Hooks
{
    /// <summary>
    ///     An object containing information about the recipient of the call.
    /// </summary>
    public sealed class To
    {
        /// <summary>
        ///     The type of the destination.
        /// </summary>
        [JsonPropertyName("type")]
        public required DestinationType Type { get; set; }

        /// <summary>
        ///     The phone number, user name, or other identifier of the destination.
        /// </summary>
        [JsonPropertyName("endpoint")]
        public required string Endpoint { get; set; }
    }
}
