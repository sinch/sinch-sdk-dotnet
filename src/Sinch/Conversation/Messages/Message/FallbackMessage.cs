using System.Text;
using System.Text.Json.Serialization;
using Sinch.Core;

namespace Sinch.Conversation.Messages.Message
{
    /// <summary>
    ///     Fallback message. Used when original contact message can not be handled.
    /// </summary>
    public sealed class FallbackMessage
    {
        /// <summary>
        ///     Optional. The raw fallback message if provided by the channel.
        /// </summary>
        [JsonPropertyName("raw_message")]
        public string? RawMessage { get; set; }


        /// <summary>
        ///     Gets or Sets Reason
        /// </summary>
        [JsonPropertyName("reason")]
        public Reason? Reason { get; set; }


        /// <summary>
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class FallbackMessage {\n");
            sb.Append("  RawMessage: ").Append(RawMessage).Append("\n");
            sb.Append("  Reason: ").Append(Reason).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }
    }

    /// <summary>
    ///     Reason
    /// </summary>
    public sealed class Reason
    {
        /// <summary>
        ///     Gets or Sets Code
        /// </summary>
        [JsonPropertyName("code")]
        public ReasonCode? Code { get; set; }


        /// <summary>
        ///     A textual description of the reason.
        /// </summary>
        [JsonPropertyName("description")]
        public string? Description { get; set; }


        /// <summary>
        ///     Gets or Sets SubCode
        /// </summary>
        [JsonPropertyName("sub_code")]
        public ReasonSubCode? SubCode { get; set; }


        /// <summary>
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class Reason {\n");
            sb.Append("  Code: ").Append(Code).Append("\n");
            sb.Append("  Description: ").Append(Description).Append("\n");
            sb.Append("  SubCode: ").Append(SubCode).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }
    }

    /// <summary>
    /// Reason for the error encountered.
    /// </summary>
    [JsonConverter(typeof(EnumRecordJsonConverter<ReasonCode>))]
    public record ReasonCode(string Value) : EnumRecord(Value)
    {
        /// <summary>
        /// UNKNOWN is used if no other code can be used to describe the encountered error.
        /// </summary>
        public static readonly ReasonCode Unknown = new("UNKNOWN");

        /// <summary>
        /// An internal error occurred. Please save the entire callback if you want to report an error.
        /// </summary>
        public static readonly ReasonCode InternalError = new("INTERNAL_ERROR");

        /// <summary>
        /// The message or event was not sent due to rate limiting.
        /// </summary>
        public static readonly ReasonCode RateLimited = new("RATE_LIMITED");

        /// <summary>
        /// The channel recipient identity was malformed.
        /// </summary>
        public static readonly ReasonCode RecipientInvalidChannelIdentity = new("RECIPIENT_INVALID_CHANNEL_IDENTITY");

        /// <summary>
        /// It was not possible to reach the contact, or channel recipient identity, on the channel.
        /// </summary>
        public static readonly ReasonCode RecipientNotReachable = new("RECIPIENT_NOT_REACHABLE");

        /// <summary>
        /// The contact, or channel recipient identity, has not opt-ed in on the channel.
        /// </summary>
        public static readonly ReasonCode RecipientNotOptedIn = new("RECIPIENT_NOT_OPTED_IN");

        /// <summary>
        /// The allowed sending window has expired. See the channel documentation for more information about how the sending window works for the different channels.
        /// </summary>
        public static readonly ReasonCode OutsideAllowedSendingWindow = new("OUTSIDE_ALLOWED_SENDING_WINDOW");

        /// <summary>
        /// The channel failed to accept the message. The Conversation API performs multiple retries in case of transient errors.
        /// </summary>
        public static readonly ReasonCode ChannelFailure = new("CHANNEL_FAILURE");

        /// <summary>
        /// The configuration of the channel for the used App is wrong. The bad configuration caused the channel to reject the message. Please see the channel support documentation page for how to set it up correctly.
        /// </summary>
        public static readonly ReasonCode ChannelBadConfiguration = new("CHANNEL_BAD_CONFIGURATION");

        /// <summary>
        /// The configuration of the channel is missing from the used App. Please see the channel support documentation page for how to set it up correctly.
        /// </summary>
        public static readonly ReasonCode ChannelConfigurationMissing = new("CHANNEL_CONFIGURATION_MISSING");

        /// <summary>
        /// Some of the referenced media files is of an unsupported media type. Please read the channel support documentation page to find out the limitations on media that the different channels impose.
        /// </summary>
        public static readonly ReasonCode MediaTypeUnsupported = new("MEDIA_TYPE_UNSUPPORTED");

        /// <summary>
        /// Some of the referenced media files are too large. Please read the channel support documentation to find out the limitations on file size that the different channels impose.
        /// </summary>
        public static readonly ReasonCode MediaTooLarge = new("MEDIA_TOO_LARGE");

        /// <summary>
        /// The provided media link was not accessible from the Conversation API or from the underlying channels. Please make sure that the media file is accessible.
        /// </summary>
        public static readonly ReasonCode MediaNotReachable = new("MEDIA_NOT_REACHABLE");

        /// <summary>
        /// No channels to try to send the message to. This error will occur if one attempts to send a message to a channel with no channel identities or if all applicable channels have been attempted.
        /// </summary>
        public static readonly ReasonCode NoChannelsLeft = new("NO_CHANNELS_LEFT");

        /// <summary>
        /// The referenced template was not found.
        /// </summary>
        public static readonly ReasonCode TemplateNotFound = new("TEMPLATE_NOT_FOUND");

        /// <summary>
        /// Sufficient template parameters was not given. All parameters defined in the template must be provided when sending a template message.
        /// </summary>
        public static readonly ReasonCode TemplateInsufficientParameters = new("TEMPLATE_INSUFFICIENT_PARAMETERS");

        /// <summary>
        /// The selected language, or version, of the referenced template did not exist. Please check the available versions and languages of the template.
        /// </summary>
        public static readonly ReasonCode TemplateNonExistingLanguageOrVersion =
            new("TEMPLATE_NON_EXISTING_LANGUAGE_OR_VERSION");

        /// <summary>
        /// The message delivery, or event delivery, failed due to a channel-imposed timeout. See the channel support documentation page for further details about how the different channels behave.
        /// </summary>
        public static readonly ReasonCode DeliveryTimedOut = new("DELIVERY_TIMED_OUT");

        /// <summary>
        /// The message or event was rejected by the channel due to a policy. Some channels have specific policies that must be met to send a message. See the channel support documentation page for more information about when this error will be triggered.
        /// </summary>
        public static readonly ReasonCode DeliveryRejectedDueToPolicy = new("DELIVERY_REJECTED_DUE_TO_POLICY");

        /// <summary>
        /// The provided Contact ID did not exist.
        /// </summary>
        public static readonly ReasonCode ContactNotFound = new("CONTACT_NOT_FOUND");

        /// <summary>
        /// Conversation API validates send requests in two different stages. The first stage is right before the message is enqueued. If this first validation fails the API responds with 400 Bad Request and the request is discarded immediately. The second validation kicks in during message processing and it normally contains channel specific validation rules. Failures during second request validation are delivered as callbacks to MESSAGE_DELIVERY (EVENT_DELIVERY) webhooks with ReasonCode BAD_REQUEST.
        /// </summary>
        public static readonly ReasonCode BadRequest = new("BAD_REQUEST");

        /// <summary>
        /// The used App is missing. This error may occur when the app is removed during message processing.
        /// </summary>
        public static readonly ReasonCode UnknownApp = new("UNKNOWN_APP");

        /// <summary>
        /// The contact has no channel identities set up, or the contact has no channels set up for the resolved channel priorities.
        /// </summary>
        public static readonly ReasonCode NoChannelIdentityForContact = new("NO_CHANNEL_IDENTITY_FOR_CONTACT");

        /// <summary>
        /// Generic error for channel permanently rejecting a message. Only used if no other better matching error can be used.
        /// </summary>
        public static readonly ReasonCode ChannelReject = new("CHANNEL_REJECT");

        /// <summary>
        /// No permission to perform action.
        /// </summary>
        public static readonly ReasonCode NoPermission = new("NO_PERMISSION");

        /// <summary>
        /// No available profile data for user.
        /// </summary>
        public static readonly ReasonCode NoProfileAvailable = new("NO_PROFILE_AVAILABLE");

        /// <summary>
        /// Generic error for channel unsupported operations.
        /// </summary>
        public static readonly ReasonCode UnsupportedOperation = new("UNSUPPORTED_OPERATION");
    }

    /// <summary>
    /// Reason sub-code for the error encountered.
    /// </summary>
    [JsonConverter(typeof(EnumRecordJsonConverter<ReasonSubCode>))]
    public record ReasonSubCode(string Value) : EnumRecord(Value)
    {
        /// <summary>
        /// UNSPECIFIED_SUB_CODE is used if no other sub code can be used to describe the encountered error.
        /// </summary>
        public static readonly ReasonSubCode UnspecifiedSubCode = new("UNSPECIFIED_SUB_CODE");

        /// <summary>
        /// The message attachment was rejected by the channel due to a policy. Some channels have specific policies that must be met to receive an attachment.
        /// </summary>
        public static readonly ReasonSubCode AttachmentRejected = new("ATTACHMENT_REJECTED");

        /// <summary>
        /// The specified media url's media type could not be determined.
        /// </summary>
        public static readonly ReasonSubCode MediaTypeUndetermined = new("MEDIA_TYPE_UNDETERMINED");

        /// <summary>
        /// The used credentials for the underlying channel is inactivated and not allowed to send or receive messages.
        /// </summary>
        public static readonly ReasonSubCode InactiveSender = new("INACTIVE_SENDER");
    }
}
