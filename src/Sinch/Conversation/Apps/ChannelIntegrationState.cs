using System.Text;
using System.Text.Json.Serialization;
using Sinch.Core;

namespace Sinch.Conversation.Apps
{
    /// <summary>
    ///     State of the channel credentials integration.
    /// </summary>
    public sealed class ChannelIntegrationState
    {
        /// <summary>
        /// Gets or Sets Status
        /// </summary>
        [JsonPropertyName("status")]
#if NET7_0_OR_GREATER
        public required ChannelIntegrationStatus Status { get; set; }
#else
        public ChannelIntegrationStatus Status { get; set; } = null!;
#endif

        /// <summary>
        ///     Description in case the integration fails
        /// </summary>
        [JsonPropertyName("description")]
        public string? Description { get; set; }


        /// <summary>
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append($"class {nameof(ChannelIntegrationState)} {{\n");
            sb.Append($"  {nameof(Status)}: ").Append(Status).Append('\n');
            sb.Append($"  {nameof(Description)}: ").Append(Description).Append('\n');
            sb.Append("}\n");
            return sb.ToString();
        }
    }

    /// <summary>
    /// Status of the channel credentials integration
    /// </summary>
    /// <value>Status of the channel credentials integration</value>
    [JsonConverter(typeof(EnumRecordJsonConverter<ChannelIntegrationStatus>))]
    public record ChannelIntegrationStatus(string Value) : EnumRecord(Value)
    {
        public static readonly ChannelIntegrationStatus Pending = new("PENDING");
        public static readonly ChannelIntegrationStatus Active = new("ACTIVE");
        public static readonly ChannelIntegrationStatus Failing = new("FAILING");
    }
}
