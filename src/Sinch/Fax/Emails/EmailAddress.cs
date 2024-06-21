using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Sinch.Fax.Emails
{
    /// <summary>
    /// Object from emails/ endoint that is used to send and recieve a fax via email
    /// </summary>
    public class EmailAddress
    {
        /// <summary>
        ///     Gets or Sets VarEmail
        /// </summary>
        [JsonPropertyName("email")]
        public string? Email { get; set; }


        /// <summary>
        ///     Numbers you want to associate with this email.
        /// </summary>
        [JsonPropertyName("phoneNumbers")]
        public List<string>? PhoneNumbers { get; set; }


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
            sb.Append($"class {nameof(Email)} {{\n");
            sb.Append($"  {nameof(Email)}: ").Append(Email).Append('\n');
            sb.Append($"  {nameof(PhoneNumbers)}: ").Append(PhoneNumbers).Append('\n');
            sb.Append($"  {nameof(ProjectId)}: ").Append(ProjectId).Append('\n');
            sb.Append("}\n");
            return sb.ToString();
        }
    }
}
