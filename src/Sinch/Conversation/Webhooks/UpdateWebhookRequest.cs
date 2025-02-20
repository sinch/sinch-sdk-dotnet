using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using Sinch.Core;

namespace Sinch.Conversation.Webhooks
{
    public sealed class UpdateWebhookRequest : PropertyMaskQuery
    {
        private WebhookTargetType? _targetType;
        private string _appId = null!;
        private ClientCredentials? _clientCredentials;
        private string? _secret;
        private string _target = null!;
        private List<WebhookTrigger> _triggers = null!;

        /// <summary>
        ///     Gets or sets the target type.
        /// </summary>
        public WebhookTargetType? TargetType
        {
            get => _targetType;
            set
            {
                SetFields.Add(nameof(TargetType));
                _targetType = value;
            }
        }

        /// <summary>
        ///     The app that this webhook belongs to.
        /// </summary>
#if NET7_0_OR_GREATER
        public required string AppId
#else
        public string AppId
#endif
        {
            get => _appId;
            set
            {
                SetFields.Add(nameof(AppId));
                _appId = value;
            }
        }

        /// <summary>
        ///     Gets or sets the client credentials.
        /// </summary>
        public ClientCredentials? ClientCredentials
        {
            get => _clientCredentials;
            set
            {
                SetFields.Add(nameof(ClientCredentials));
                _clientCredentials = value;
            }
        }

        /// <summary>
        ///     Optional secret to be used to sign contents of webhooks sent by the Conversation API.
        ///     You can then use the secret to verify the signature.
        /// </summary>
        public string? Secret
        {
            get => _secret;
            set
            {
                SetFields.Add(nameof(Secret));
                _secret = value;
            }
        }

        /// <summary>
        ///     Gets or sets the target URL where events should be sent to.
        ///     Maximum URL length is 742. The conversation-api.*.sinch.com subdomains are forbidden.
        /// </summary>
#if NET7_0_OR_GREATER
        public required string Target
#else
        public string Target
#endif
        {
            get => _target;
            set
            {
                SetFields.Add(nameof(Target));
                _target = value;
            }
        }

        /// <summary>
        ///     An array of triggers that should trigger the webhook and result in an event being sent to the target URL.
        ///     Refer to the list of [Webhook Triggers](https://developers.sinch.com/docs/conversation/callbacks#webhook-triggers)
        ///     for a complete list.
        /// </summary>
#if NET7_0_OR_GREATER
        public required List<WebhookTrigger> Triggers
#else
        public List<WebhookTrigger> Triggers
#endif
        {
            get => _triggers;
            set
            {
                SetFields.Add(nameof(Triggers));
                _triggers = value;
            }
        }

        /// <summary>
        ///     Returns the string presentation of the object.
        /// </summary>
        /// <returns>String presentation of the object.</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class Webhook {\n");
            sb.Append("  AppId: ").Append(AppId).Append("\n");
            sb.Append("  ClientCredentials: ").Append(ClientCredentials).Append("\n");
            sb.Append("  Target: ").Append(Target).Append("\n");
            sb.Append("  TargetType: ").Append(TargetType).Append("\n");
            sb.Append("  Triggers: ").Append(Triggers).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }
    }
}
