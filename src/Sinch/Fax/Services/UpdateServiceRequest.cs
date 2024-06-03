using System.Text;
using System.Text.Json.Serialization;
using Sinch.Fax.Faxes;

namespace Sinch.Fax.Services
{
    public sealed class UpdateServiceRequest
    {
        /// <summary>
        /// Determines how documents are converted to black and white. Value should be halftone or monochrome. Defaults to value selected on Fax Settings page
        /// </summary>
        [JsonPropertyName("imageConversionMethod")]
        public ImageConversionMethod? ImageConversionMethod { get; set; }

        /// <summary>
        /// The content type of the webhook.
        /// </summary>
        [JsonPropertyName("webhookContentType")]
        public CallbackUrlContentType? WebhookContentType { get; set; }


        /// <summary>
        ///     ID of the fax service used.
        /// </summary>
        [JsonPropertyName("id")]
#if NET7_0_OR_GREATER
        public required string Id { get; set; } = null!;
#else

        public string Id { get; set; } = null!;
#endif


        /// <summary>
        ///     A friendly name for the service. Maximum is 60 characters.
        /// </summary>
        [JsonPropertyName("name")]
        public string? Name { get; set; }


        /// <summary>
        ///     The URL to which Sinch will post when someone sends a fax to your Sinch number. To accept incoming faxes this must be set and your Sinch phone number must be configured to receive faxes.
        /// </summary>
        [JsonPropertyName("incomingWebhookUrl")]
        public string? IncomingWebhookUrl { get; set; }


        /// <summary>
        ///     If set to true this is the service used to create faxes when no serviceId is specified in the API endpoints.
        /// </summary>
        [JsonPropertyName("defaultForProject")]
        public bool? DefaultForProject { get; set; }


        /// <summary>
        ///     One of your sinch numbers connected to this service or any of your verified numbers
        /// </summary>
        [JsonPropertyName("defaultFrom")]
        public string? DefaultFrom { get; set; }


        /// <summary>
        ///     The number of times to retry sending a fax if it fails. Default is 3. Maximum is 5.
        /// </summary>
        [JsonPropertyName("numberOfRetries")]
        public int? NumberOfRetries { get; set; }


        /// <summary>
        ///     The number of seconds to wait between retries if the fax is not yet completed.
        /// </summary>
        [JsonPropertyName("retryDelaySeconds")]
        public int? RetryDelaySeconds { get; set; }


        /// <summary>
        ///     Save fax documents with sinch when you send faxes
        /// </summary>
        [JsonPropertyName("saveOutboundFaxDocuments")]
        public bool? SaveOutboundFaxDocuments { get; set; }


        /// <summary>
        ///     Save fax documents with sinch when you receive faxes
        /// </summary>
        [JsonPropertyName("saveInboundFaxDocuments")]
        public bool? SaveInboundFaxDocuments { get; set; }


        /// <summary>
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append($"class {nameof(Service)} {{\n");
            sb.Append($"  {nameof(Id)}: ").Append(Id).Append('\n');
            sb.Append($"  {nameof(Name)}: ").Append(Name).Append('\n');
            sb.Append($"  {nameof(IncomingWebhookUrl)}: ").Append(IncomingWebhookUrl).Append('\n');
            sb.Append($"  {nameof(WebhookContentType)}: ").Append(WebhookContentType).Append('\n');
            sb.Append($"  {nameof(DefaultForProject)}: ").Append(DefaultForProject).Append('\n');
            sb.Append($"  {nameof(DefaultFrom)}: ").Append(DefaultFrom).Append('\n');
            sb.Append($"  {nameof(NumberOfRetries)}: ").Append(NumberOfRetries).Append('\n');
            sb.Append($"  {nameof(RetryDelaySeconds)}: ").Append(RetryDelaySeconds).Append('\n');
            sb.Append($"  {nameof(ImageConversionMethod)}: ").Append(ImageConversionMethod).Append('\n');
            sb.Append($"  {nameof(SaveOutboundFaxDocuments)}: ").Append(SaveOutboundFaxDocuments).Append('\n');
            sb.Append($"  {nameof(SaveInboundFaxDocuments)}: ").Append(SaveInboundFaxDocuments).Append('\n');
            sb.Append("}\n");
            return sb.ToString();
        }
    }
}
