using System.Text;
using System.Text.Json.Serialization;

namespace Sinch.Fax.Emails
{
    public sealed class ServicePhoneNumber
    {
        /// <summary>
        ///     A phone number in [E.164](https://community.sinch.com/t5/Glossary/E-164/ta-p/7537) format, including the leading &#39;+&#39;.
        /// </summary>
        [JsonPropertyName("phoneNumber")]
        public string? PhoneNumber { get; set; }


        /// <summary>
        ///     The &#x60;Id&#x60; of the project associated with the call.
        /// </summary>
        [JsonPropertyName("projectId")]
        public string? ProjectId { get; set; }


        /// <summary>
        ///     ID of the fax service used.
        /// </summary>
        [JsonPropertyName("serviceId")]
        public string? ServiceId { get; set; }


        /// <summary>
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append($"class {nameof(ServicePhoneNumber)} {{\n");
            sb.Append($"  {nameof(PhoneNumber)}: ").Append(PhoneNumber).Append('\n');
            sb.Append($"  {nameof(ProjectId)}: ").Append(ProjectId).Append('\n');
            sb.Append($"  {nameof(ServiceId)}: ").Append(ServiceId).Append('\n');
            sb.Append("}\n");
            return sb.ToString();
        }
    }
}
