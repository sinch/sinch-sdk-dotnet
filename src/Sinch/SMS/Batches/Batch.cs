using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using Sinch.SMS.Batches.Send;

namespace Sinch.SMS.Batches
{
    /// <summary>
    ///     Marker interface for batch 
    /// </summary>
    [JsonConverter(typeof(BatchConverter))]
    public interface IBatch
    {
        /// <summary>
        ///     Unique identifier for batch
        /// </summary>
        public string? Id { get; set; }

        /// <summary>
        ///     Indicates if the batch has been canceled or not.
        /// </summary>
        public bool? Canceled { get; set; }
    }

    internal sealed class BatchConverter : JsonConverter<IBatch>
    {
        public override IBatch Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var elem = JsonElement.ParseValue(ref reader);
            var descriptor = elem.EnumerateObject().FirstOrDefault(x => x.Name == "type");
            var type = descriptor.Value.GetString();
            if (type == SmsType.MtText.Value)
            {
                return elem.Deserialize<TextBatch>(options) ??
                       throw new InvalidOperationException($"{nameof(TextBatch)} deserialization result is null.");
            }

            if (type == SmsType.MtBinary.Value)
            {
                return elem.Deserialize<BinaryBatch>(options) ??
                       throw new InvalidOperationException($"{nameof(TextBatch)} deserialization result is null.");
            }

            if (type == SmsType.MtMedia.Value)
            {
                return elem.Deserialize<MediaBatch>(options) ??
                       throw new InvalidOperationException($"{nameof(TextBatch)} deserialization result is null.");
            }

            throw new JsonException($"Failed to match verification method object, got {descriptor.Name}");
        }

        public override void Write(Utf8JsonWriter writer, IBatch value, JsonSerializerOptions options)
        {
            switch (value)
            {
                case BinaryBatch batch:
                    JsonSerializer.Serialize(writer, batch, options);
                    break;
                case MediaBatch batch:
                    JsonSerializer.Serialize(writer, batch, options);
                    break;
                case TextBatch batch:
                    JsonSerializer.Serialize(writer, batch, options);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(value),
                        $"Cannot find a proper specific type for {nameof(IBatch)}");
            }
        }
    }

    public sealed class TextBatch : BatchBase, IBatch
    {
        /// <summary>
        ///     Unique identifier for batch
        /// </summary>
        public string? Id { get; set; }

        /// <summary>
        ///     Indicates if the batch has been canceled or not.
        /// </summary>
        public bool? Canceled { get; set; }

        /// <summary>
        ///     The message content
        /// </summary>

        public required string Body { get; set; }



        /// <summary>
        ///     Contains the parameters that will be used for customizing the message for each recipient.<br /><br />
        ///     <see href="https://developers.sinch.com/docs/sms/resources/message-info/message-parameterization">
        ///         Click here to
        ///         learn more about parameterization.
        ///     </see>
        /// </summary>
        public Dictionary<string, Dictionary<string, string>>? Parameters { get; set; }

        /// <summary>
        ///     Regular SMS
        /// </summary>
        public override SmsType Type { get; } = SmsType.MtText;

        /// <summary>
        ///     Shows message on screen without user interaction while not saving the message to the inbox.
        /// </summary>
        public bool? FlashMessage { get; set; }

        /// <summary>
        ///     If set to true the message will be shortened when exceeding one part.
        /// </summary>
        public bool? TruncateConcat { get; set; }

        /// <summary>
        ///     Message will be dispatched only if it is not split to more parts than Max Number of Message Parts
        /// </summary>
        public int? MaxNumberOfMessageParts { get; set; }

        /// <summary>
        ///     The type of number for the sender number. Use to override the automatic detection.
        /// </summary>
        public int? FromTon { get; set; }

        /// <summary>
        ///     Number Plan Indicator for the sender number. Use to override the automatic detection.
        /// </summary>
        public int? FromNpi { get; set; }
    }


    public sealed class BinaryBatch : BatchBase, IBatch
    {
        /// <summary>
        ///     Unique identifier for batch
        /// </summary>
        public string? Id { get; set; }

        /// <summary>
        ///     Indicates if the batch has been canceled or not.
        /// </summary>
        public bool? Canceled { get; set; }

        /// <summary>
        ///       The UDH header of a binary message HEX encoded. Max 140 bytes including the <c>body</c>.  
        /// </summary>
        public string? Udh { get; set; }

        /// <summary>
        ///     Shows message on screen without user interaction while not saving the message to the inbox.
        /// </summary>
        public bool? FlashMessage { get; set; }

        /// <summary>
        ///     If set to true the message will be shortened when exceeding one part.
        /// </summary>
        public bool? TruncateConcat { get; set; }

        /// <summary>
        ///     Message will be dispatched only if it is not split to more parts than Max Number of Message Parts
        /// </summary>
        public int? MaxNumberOfMessageParts { get; set; }

        /// <summary>
        ///     The type of number for the sender number. Use to override the automatic detection.
        /// </summary>
        public int? FromTon { get; set; }

        /// <summary>
        ///     Number Plan Indicator for the sender number. Use to override the automatic detection.
        /// </summary>
        public int? FromNpi { get; set; }

        /// <summary>
        ///     The message content Base64 encoded.<br/><br/>
        ///     Max 140 bytes including <see cref="Udh"/>.
        /// </summary>

        public required string Body { get; set; }


        /// <summary>
        ///         SMS in binary format.
        /// </summary>
        public override SmsType Type { get; } = SmsType.MtBinary;
    }

    public sealed class MediaBatch : BatchBase, IBatch
    {
        /// <summary>
        ///     Unique identifier for batch
        /// </summary>
        public string? Id { get; set; }

        /// <summary>
        ///     Indicates if the batch has been canceled or not.
        /// </summary>
        public bool? Canceled { get; set; }

        /// <summary>
        ///     The message content, including a URL to the media file
        /// </summary>

        public required MediaBody Body { get; set; }

        /// <summary>
        ///     MMS
        /// </summary>
        public override SmsType Type { get; } = SmsType.MtMedia;

        /// <summary>
        ///     Default: <c>false</c>.<br/><br/>
        ///     Whether or not you want the media included in your message to be checked against
        ///     <see href="https://developers.sinch.com/docs/mms/bestpractices/">Sinch MMS channel best practices</see>.
        ///     If set to true, your message will be rejected if it doesn't conform to the listed
        ///     recommendations, otherwise no validation will be performed.
        /// </summary>
        public bool? StrictValidation { get; set; }

        /// <summary>
        ///     Contains the parameters that will be used for customizing the message for each recipient.<br /><br />
        ///     <see href="https://developers.sinch.com/docs/sms/resources/message-info/message-parameterization">
        ///         Click here to
        ///         learn more about parameterization.
        ///     </see>
        /// </summary>
        public Dictionary<string, Dictionary<string, string>>? Parameters { get; set; }
    }
}
