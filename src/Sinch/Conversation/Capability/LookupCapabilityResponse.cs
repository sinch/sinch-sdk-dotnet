using Sinch.Conversation.Common;

namespace Sinch.Conversation.Capability
{
    public class LookupCapabilityResponse
    {
        /// <summary>
        ///     The ID of the app to use for capability lookup.
        /// </summary>

        public required string AppId { get; set; }


        /// <summary>
        ///     The recipient to lookup capabilities for. Requires either contact_id or identified_by.
        /// </summary>

        public required IRecipient Recipient { get; set; }


        /// <summary>
        ///     ID for the asynchronous response, will be generated if not set. Currently this field is not used for idempotency.
        /// </summary>
        public string? RequestId { get; set; }
    }
}
