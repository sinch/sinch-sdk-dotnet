using System.Text;
using System.Text.Json.Serialization;

namespace Sinch.Fax.Services
{
    public sealed class UpdateFaxServiceRequest : ServiceBase
    {
        /// <summary>
        ///     ID of the fax service used.
        /// </summary>
        [JsonPropertyName("id")]

        public required string Id { get; set; } = null!;



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
            sb.Append($"  {nameof(IncomingEventDestinationUrl)}: ").Append(IncomingEventDestinationUrl).Append('\n');
            sb.Append($"  {nameof(EventDestinationContentType)}: ").Append(EventDestinationContentType).Append('\n');
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
