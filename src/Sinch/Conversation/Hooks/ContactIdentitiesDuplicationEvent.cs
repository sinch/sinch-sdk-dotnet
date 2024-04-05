using System;
using System.Text;
using System.Text.Json.Serialization;
using Sinch.Conversation.Hooks.Models;

namespace Sinch.Conversation.Hooks
{
    /// <summary>
    ///     This callback is sent when duplicates of channel identities are found between multiple contacts in the contact database during message and event processing.
    /// </summary>
    public sealed class ContactIdentitiesDuplicationEvent : CallbackEventBase
    {
        /// <summary>
        ///     Gets or Sets DuplicatedContactIdentitiesNotification
        /// </summary>
        [JsonPropertyName("duplicated_contact_identities_notification")]
        public DuplicatedIdentitiesEvent DuplicatedContactIdentitiesNotification { get; set; }


        /// <summary>
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append($"class {nameof(ContactIdentitiesDuplicationEvent)} {{\n");
            sb.Append($"  {nameof(AppId)}: ").Append(AppId).Append('\n');
            sb.Append($"  {nameof(AcceptedTime)}: ").Append(AcceptedTime).Append('\n');
            sb.Append($"  {nameof(EventTime)}: ").Append(EventTime).Append('\n');
            sb.Append($"  {nameof(ProjectId)}: ").Append(ProjectId).Append('\n');
            sb.Append($"  {nameof(MessageMetadata)}: ").Append(MessageMetadata).Append('\n');
            sb.Append($"  {nameof(CorrelationId)}: ").Append(CorrelationId).Append('\n');
            sb.Append($"  {nameof(DuplicatedContactIdentitiesNotification)}: ").Append(DuplicatedContactIdentitiesNotification).Append('\n');
            sb.Append("}\n");
            return sb.ToString();
        }

    }

}
