using System.Text;
using System.Text.Json.Serialization;

namespace Sinch.Fax.Services
{
    /// <summary>
    ///     You can use the default created service, or create multiple services within the same project to have different default behavior for all your different faxing use cases.
    /// </summary>
    public sealed class Service : ServiceBase
    {
        /// <summary>
        ///     ID of the fax service used.
        /// </summary>
        [JsonPropertyName("id")]
        public string? Id { get; set; }


        /// <summary>
        ///     The &#x60;Id&#x60; of the project associated with the call.
        /// </summary>
        [JsonPropertyName("projectId")]
        public string? ProjectId { get; set; }


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
            sb.Append($"  {nameof(ProjectId)}: ").Append(ProjectId).Append('\n');
            sb.Append("}\n");
            return sb.ToString();
        }
    }
}
