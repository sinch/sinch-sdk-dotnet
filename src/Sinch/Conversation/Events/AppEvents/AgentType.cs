using System.Text.Json.Serialization;
using Sinch.Core;

namespace Sinch.Conversation.Events.AppEvents
{
    [JsonConverter(typeof(EnumRecordJsonConverter<AgentType>))]
    public record AgentType(string Value) : EnumRecord(Value)
    {
        /// <summary>
        ///     The default AgentType. When the agent type is unknown.
        /// </summary>
        public static readonly AgentType UnknownAgentType = new("UNKNOWN_AGENT_TYPE");

        /// <summary>
        ///     Human agent.
        /// </summary>
        public static readonly AgentType Human = new("HUMAN");

        /// <summary>
        ///     Bot agent.
        /// </summary>
        public static readonly AgentType Bot = new("BOT");
    }
}
