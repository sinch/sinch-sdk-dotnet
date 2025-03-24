using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Sinch.SMS.Batches.Send;

namespace Sinch.SMS.Batches.Update
{
    [JsonConverter(typeof(UpdateBatchConverter))]
    public interface IUpdateBatchRequest
    {
    }

    public sealed class UpdateBatchConverter : JsonConverter<IUpdateBatchRequest>
    {
        public override IUpdateBatchRequest Read(ref Utf8JsonReader reader, Type typeToConvert,
            JsonSerializerOptions options)
        {
            var elem = JsonElement.ParseValue(ref reader);
            var descriptor = elem.EnumerateObject().FirstOrDefault(x => x.Name == "type");
            var type = descriptor.Value.GetString();
            if (type == SmsType.MtText.Value)
            {
                return elem.Deserialize<UpdateTextBatchRequest>(options) ??
                       throw new InvalidOperationException(
                           $"{nameof(UpdateTextBatchRequest)} deserialization result is null.");
            }

            if (type == SmsType.MtBinary.Value)
            {
                return elem.Deserialize<UpdateBinaryBatchRequest>(options) ??
                       throw new InvalidOperationException(
                           $"{nameof(UpdateBinaryBatchRequest)} deserialization result is null.");
            }

            if (type == SmsType.MtMedia.Value)
            {
                return elem.Deserialize<UpdateMediaBatchRequest>(options) ??
                       throw new InvalidOperationException(
                           $"{nameof(UpdateMediaBatchRequest)} deserialization result is null.");
            }

            throw new JsonException($"Failed to match verification method object, got {descriptor.Name}");
        }

        public override void Write(Utf8JsonWriter writer, IUpdateBatchRequest value,
            JsonSerializerOptions options)
        {
            switch (value)
            {
                case UpdateBinaryBatchRequest binaryBatchRequest:
                    JsonSerializer.Serialize(writer, binaryBatchRequest, options);
                    break;
                case UpdateMediaBatchRequest mediaBatchRequest:
                    JsonSerializer.Serialize(writer, mediaBatchRequest, options);
                    break;
                case UpdateTextBatchRequest textBatchRequest:
                    JsonSerializer.Serialize(writer, textBatchRequest, options);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(value),
                        $"Cannot find a proper specific type for {nameof(ISendBatchRequest)}");
            }
        }
    }

    public abstract class UpdateBatchBaseRequest
    {
        /// <summary>
        ///     Sender number. Must be valid phone number, short code or alphanumeric.
        ///     Required if Automatic Default Originator not configured.
        /// </summary>
        public string? From { get; set; }

        public abstract SmsType Type { get; }

        /// <summary>
        ///     List of phone numbers and group IDs to add to the batch.
        /// </summary>
        public List<string>? ToAdd { get; set; }

        /// <summary>
        ///     List of phone numbers and group IDs to remove from the batch.
        /// </summary>
        public List<string>? ToRemove { get; set; }

        /// <summary>
        ///     Request delivery report callback.<br/><br/>
        ///     Note that delivery reports can be fetched from the API regardless of this setting.
        /// </summary>
        public DeliveryReport? DeliveryReport { get; set; }

        /// <summary>
        ///     If set in the future, the message will be delayed until send_at occurs. Must be before expire_at.
        ///     If set in the past, messages will be sent immediately.
        /// </summary>
        public DateTime? SendAt { get; set; }

        /// <summary>
        ///     If set, the system will stop trying to deliver the message at this point. Must be after send_at.
        ///     Default and max is 3 days after send_at.
        /// </summary>
        public DateTime? ExpireAt { get; set; }

        /// <summary>
        ///     Override the default callback URL for this batch. Must be a valid URL.
        ///     Learn how to set a default callback URL
        ///     <see href="https://community.sinch.com/t5/SMS/How-do-I-assign-a-callback-URL-to-an-SMS-service-plan/ta-p/8414">here</see>
        ///     .
        /// </summary>
        public Uri? CallbackUrl { get; set; }
        
        
        /// <summary>
        ///     The client identifier of a batch message. If set, the identifier will be added in the delivery report/callback of this batch
        /// </summary>
        [JsonPropertyName("client_reference")]
        public string? ClientReference { get; set; }
        
        /// <summary>
        ///     If set to &#x60;true&#x60;, then [feedback](/docs/sms/api-reference/sms/tag/Batches/#tag/Batches/operation/deliveryFeedback) is expected after successful delivery.
        /// </summary>
        [JsonPropertyName("feedback_enabled")]
        public bool? FeedbackEnabled { get; set; }
       
    }
}
