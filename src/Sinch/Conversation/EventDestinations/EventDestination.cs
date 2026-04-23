using System.Collections.Generic;
using System.Text;

namespace Sinch.Conversation.EventDestinations
{
    /// <summary>
    ///     Represents a destination for receiving event destinations from the Conversation API.
    /// </summary>
    public sealed class EventDestination
    {
        /// <summary>
        ///     Gets or sets the target type.
        /// </summary>
        public EventDestinationTargetType? TargetType { get; init; }

        /// <summary>
        ///     The app that this event destination belongs to.
        /// </summary>
        public string? AppId { get; init; }

        /// <summary>
        ///     Gets or sets the client credentials.
        /// </summary>
        public ClientCredentials? ClientCredentials { get; init; }

        /// <summary>
        ///     Gets or sets the ID of the event destination.
        /// </summary>
        public string? Id { get; init; }

        /// <summary>
        ///     Optional secret to be used to sign contents of event destinations sent by the Conversation API.
        ///     You can then use the secret to verify the signature.
        /// </summary>
        public string? Secret { get; init; }

        /// <summary>
        ///     Gets or sets the target URL where events should be sent to.
        ///     Maximum URL length is 742. The conversation-api.*.sinch.com subdomains are forbidden.
        /// </summary>
        public required string Target { get; init; }

        /// <summary>
        ///     An array of triggers that should trigger the event destination and result in an event being sent to the target URL.
        ///     Refer to the list of [EventDestination Triggers](https://developers.sinch.com/docs/conversation/callbacks#webhook-triggers)
        ///     for a complete list.
        /// </summary>
        public List<EventDestinationTrigger>? Triggers { get; init; }

        /// <summary>
        ///     Returns the string presentation of the object.
        /// </summary>
        /// <returns>String presentation of the object.</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class EventDestination {\n");
            sb.Append("  AppId: ").Append(AppId).Append("\n");
            sb.Append("  ClientCredentials: ").Append(ClientCredentials).Append("\n");
            sb.Append("  Id: ").Append(Id).Append("\n");
            sb.Append("  Target: ").Append(Target).Append("\n");
            sb.Append("  TargetType: ").Append(TargetType).Append("\n");
            sb.Append("  Triggers: ").Append(Triggers).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }
    }
}
