using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using Sinch.Voice.Applications.GetNumbers;

namespace Sinch.Voice.Applications.UpdateNumbers
{
    public class AssignNumbersRequest
    {
        /// <summary>
        ///     Indicates the DID capability that needs to be assigned to the chosen application. Valid values are &#39;voice&#39;
        ///     and &#39;sms&#39;. Please note that the DID needs to support the selected capability.
        /// </summary>
        public Capability Capability { get; set; }

        /// <summary>
        ///     The phone number or list of numbers in E.164 format.
        /// </summary>
        public List<string> Numbers { get; set; }


        /// <summary>
        ///     Indicates the application where the number(s) will be assigned. If empty, the application key that is used to sign
        ///     the request will be used.
        /// </summary>
        [JsonPropertyName("applicationkey")]
        public string ApplicationKey { get; set; }


        /// <summary>
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class AssignNumbersRequest {\n");
            sb.Append("  Numbers: ").Append(Numbers).Append("\n");
            sb.Append("  Capability: ").Append(Capability).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }
    }
}
