using System.Text.Json.Serialization;

namespace Sinch.Conversation.Common
{
    /// <summary>
    ///     Represents an agent that is involved in a conversation.
    /// </summary>
    public class Agent
    {
        /// <summary>
        ///     Agent's display name
        /// </summary>
        [JsonPropertyName("display_name")]
        public string? DisplayName { get; set; }

        [JsonPropertyName("type")]
        public AgentType? Type { get; set; }

        /// <summary>
        ///     The Agent's picture url.
        /// </summary>
        [JsonPropertyName("picture_url")]
        public string? PictureUrl { get; set; }
    }
}
