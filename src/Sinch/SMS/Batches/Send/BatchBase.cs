using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Sinch.SMS.Batches.Send
{
    /// <summary>
    ///     Marker interface for batch messages
    /// </summary>
    [JsonConverter(typeof(SendBatchRequestConverter))]
    public interface ISendBatchRequest
    {
    }

    public class SendBatchRequestConverter : JsonConverter<ISendBatchRequest>
    {
        public override ISendBatchRequest Read(ref Utf8JsonReader reader, Type typeToConvert,
            JsonSerializerOptions options)
        {
            var elem = JsonElement.ParseValue(ref reader);
            var descriptor = elem.EnumerateObject().FirstOrDefault(x => x.Name == "type");
            var type = descriptor.Value.GetString();
            if (type == SmsType.MtText.Value)
            {
                return elem.Deserialize<SendTextBatchRequest>(options);
            }

            if (type == SmsType.MtBinary.Value)
            {
                return elem.Deserialize<SendBinaryBatchRequest>(options);
            }

            if (type == SmsType.MtMedia.Value)
            {
                return elem.Deserialize<SendMediaBatchRequest>(options);
            }

            throw new JsonException($"Failed to match verification method object, got {descriptor.Name}");
        }

        public override void Write(Utf8JsonWriter writer, ISendBatchRequest value,
            JsonSerializerOptions options)
        {
            switch (value)
            {
                case SendBinaryBatchRequest binaryBatchRequest:
                    JsonSerializer.Serialize(writer, binaryBatchRequest, options);
                    break;
                case SendMediaBatchRequest mediaBatchRequest:
                    JsonSerializer.Serialize(writer, mediaBatchRequest, options);
                    break;
                case SendTextBatchRequest textBatchRequest:
                    JsonSerializer.Serialize(writer, textBatchRequest, options);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(value),
                        $"Cannot find a proper specific type for {nameof(ISendBatchRequest)}");
            }
        }
    }

    public abstract class BatchBase
    {
        /// <summary>
        ///     List of Phone numbers and group IDs that will receive the batch.
        ///     <see href="https://community.sinch.com/t5/Glossary/MSISDN/ta-p/7628">More info</see>
        /// </summary>
#if NET7_0_OR_GREATER
        public required List<string> To { get; set; }
#else
        public List<string> To { get; set; }
#endif

        /// <summary>
        ///     Sender number. Must be valid phone number, short code or alphanumeric.
        ///     Required if Automatic Default Originator not configured.
        /// </summary>
        public string From { get; set; }

        public abstract SmsType Type { get; }

        /// <summary>
        ///     Request delivery report callback.<br/><br/>
        ///     Note that delivery reports can be fetched from the API regardless of this setting.
        /// </summary>
        public DeliveryReport DeliveryReport { get; set; }

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
        public Uri CallbackUrl { get; set; }

        /// <summary>
        ///     The client identifier of a batch message.
        ///     If set, the identifier will be added in the delivery report/callback of this batch
        /// </summary>
        public string ClientReference { get; set; }

        /// <summary>
        ///     If set to <c>true</c>, then
        ///     <see
        ///         href="https://developers.sinch.com/docs/sms/api-reference/sms/tag/Batches/#tag/Batches/operation/deliveryFeedback">
        ///         feedback
        ///     </see>
        ///     is expected after successful delivery.
        /// </summary>
        public bool FeedbackEnabled { get; set; } = false;
    }
}
