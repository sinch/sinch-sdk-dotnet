using System.Text;
using System.Text.Json.Serialization;

namespace Sinch.Conversation.Hooks.Models
{
    /// <summary>
    ///     The content of the event when contact_event is not populated. Note that this object is currently only available to select customers for beta testing. Mutually exclusive with contact_event.
    /// </summary>
    public sealed class ContactMessageEvent
    {
        /// <summary>
        ///     Gets or Sets PaymentStatusUpdateEvent
        /// </summary>
        [JsonPropertyName("payment_status_update_event")]
        public ContactMessageEventPaymentStatusUpdateEvent PaymentStatusUpdateEvent { get; set; }


        /// <summary>
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append($"class {nameof(ContactMessageEvent)} {{\n");
            sb.Append($"  {nameof(PaymentStatusUpdateEvent)}: ").Append(PaymentStatusUpdateEvent).Append('\n');
            sb.Append("}\n");
            return sb.ToString();
        }
    }
}
