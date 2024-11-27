using System.Text;

namespace Sinch.Fax.Services
{
    public sealed class CreateServiceRequest : ServiceBase
    {
        /// <summary>
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append($"class {nameof(Service)} {{\n");
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
