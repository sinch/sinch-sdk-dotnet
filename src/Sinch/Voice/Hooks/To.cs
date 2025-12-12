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
#if NET7_0_OR_GREATER
        public required DestinationType Type { get; set; }
#else
        public DestinationType Type { get; set; } = null!;
#endif

        /// <summary>
        ///     The phone number, user name, or other identifier of the destination.
        /// </summary>
        [JsonPropertyName("endpoint")]
#if NET7_0_OR_GREATER
        public required string Endpoint { get; set; }
#else
        public string Endpoint { get; set; } = null!;
#endif
    }
}
