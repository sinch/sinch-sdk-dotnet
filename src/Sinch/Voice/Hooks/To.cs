using System.Text.Json.Serialization;

namespace Sinch.Voice.Hooks
{
    /// <summary>
    ///     An object containing information about the recipient of the call.
    /// </summary>
    public class To
    {
        /// <summary>
        ///     The type of the destination.
        /// </summary>
        [JsonPropertyName("type")]
        public string Type { get; set; }

        /// <summary>
        ///     The phone number, user name, or other identifier of the destination.
        /// </summary>
        [JsonPropertyName("endpoint")]
        public string Endpoint { get; set; }
    }
}
