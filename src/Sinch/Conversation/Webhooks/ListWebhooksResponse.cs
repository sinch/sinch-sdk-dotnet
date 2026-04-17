using System.Collections.Generic;

namespace Sinch.Conversation.Webhooks
{
    public sealed class ListWebhooksResponse
    {
        public IEnumerable<Webhook>? Webhooks { get; set; }
    }
}
