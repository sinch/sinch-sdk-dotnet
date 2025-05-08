using Sinch.Conversation.Common;

namespace Sinch.Conversation.Capability
{
    public sealed class LookupCapabilityResponse
    {
        /// <summary>
        ///     The ID of the app to use for capability lookup.
        /// </summary>
#if NET7_0_OR_GREATER
        public required string AppId { get; set; }
#else
        public string AppId { get; set; } = null!;
#endif

        /// <summary>
        ///     The recipient to lookup capabilities for. Requires either contact_id or identified_by.
        /// </summary>
#if NET7_0_OR_GREATER
        public required IRecipient Recipient { get; set; }
#else
        public IRecipient Recipient { get; set; } = null!;
#endif

        /// <summary>
        ///     ID for the asynchronous response, will be generated if not set. Currently this field is not used for idempotency.
        /// </summary>
        public string? RequestId { get; set; }
    }
}
