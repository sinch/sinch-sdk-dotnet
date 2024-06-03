using System.Text;
using System.Text.Json.Serialization;

namespace Sinch.Fax.Services
{
    public sealed class UpdateServiceRequest : ServiceBase
    {
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
