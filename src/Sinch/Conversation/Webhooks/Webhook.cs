using System.Collections.Generic;
using System.Text;

namespace Sinch.Conversation.Webhooks
{
    /// <summary>
    ///     Represents a destination for receiving callbacks from the Conversation API.
    /// </summary>
    public class Webhook
    {
        /// <summary>
        ///     Gets or Sets TargetType
        /// </summary>
        public WebhookTargetType TargetType { get; set; }

        /// <summary>
        ///     The app that this webhook belongs to.
        /// </summary>
#if NET7_0_OR_GREATER
        public required string AppId { get; set; }
#else
        public string AppId { get; set; }
#endif


        /// <summary>
        ///     Gets or Sets ClientCredentials
        /// </summary>
        public ClientCredentials ClientCredentials { get; set; }


        /// <summary>
        ///     The ID of the webhook.
        /// </summary>
        public string Id { get; }


        /// <summary>
        ///     Optional secret be used to sign contents of webhooks sent by the Conversation API. You can then use the secret to
        ///     verify the signature.
        /// </summary>
        public string Secret { get; set; }


        /// <summary>
        ///     The target url where events should be sent to. Maximum URL length is 742. The conversation-api.*.sinch.com
        ///     subdomains are forbidden.
        /// </summary>
#if NET7_0_OR_GREATER
        public required string Target { get; set; }
#else
        public string Target { get; set; }
#endif


        /// <summary>
        ///     An array of triggers that should trigger the webhook and result in an event being sent to the target url. Refer to
        ///     the list of [Webhook Triggers](/docs/conversation/callbacks#webhook-triggers) for a complete list.
        /// </summary>
#if NET7_0_OR_GREATER
        public required List<WebhookTrigger> Triggers { get; set; }
#else
        public List<WebhookTrigger> Triggers { get; set; }
#endif


        /// <summary>
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class Webhook {\n");
            sb.Append("  AppId: ").Append(AppId).Append("\n");
            sb.Append("  ClientCredentials: ").Append(ClientCredentials).Append("\n");
            sb.Append("  Id: ").Append(Id).Append("\n");
            sb.Append("  Secret: ").Append(Secret).Append("\n");
            sb.Append("  Target: ").Append(Target).Append("\n");
            sb.Append("  TargetType: ").Append(TargetType).Append("\n");
            sb.Append("  Triggers: ").Append(Triggers).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }
    }
}
