using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using Sinch.Core;

namespace Sinch.Voice.Applications.GetNumbers
{
    public class GetNumbersResponse
    {
        /// <summary>
        ///     The object type. Will always be list of numbers, associated application keys and capabilities
        /// </summary>
        public List<NumberItem> Numbers { get; set; }
    }

    public sealed class NumberItem
    {
        /// <summary>
        ///     Indicates the DID capability that needs to be assigned to the chosen application. Valid values are &#39;voice&#39;
        ///     and &#39;sms&#39;. Please note that the DID needs to support the selected capability.
        /// </summary>
        public Capability Capability { get; set; }

        /// <summary>
        ///     Numbers that you own in E.164 format.
        /// </summary>
        public string Number { get; set; }


        /// <summary>
        ///     Indicates the application where the number(s) will be assigned. If no number is assigned the applicationkey will
        ///     not be returned.
        /// </summary>
        public string Applicationkey { get; set; }


        /// <summary>
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class GetNumbersResponseObjNumbersInner {\n");
            sb.Append("  Number: ").Append(Number).Append("\n");
            sb.Append("  Applicationkey: ").Append(Applicationkey).Append("\n");
            sb.Append("  Capability: ").Append(Capability).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }
    }

    /// <summary>
    ///     Indicates the DID capability that needs to be assigned to the chosen application. Valid values are &#39;voice&#39;
    ///     and &#39;sms&#39;. Please note that the DID needs to support the selected capability.
    /// </summary>
    [JsonConverter(typeof(EnumRecordJsonConverter<Capability>))]
    public record Capability(string Value) : EnumRecord(Value)
    {
        public static readonly Capability Voice = new("voice");
        public static readonly Capability Sms = new("sms");
    }
}
