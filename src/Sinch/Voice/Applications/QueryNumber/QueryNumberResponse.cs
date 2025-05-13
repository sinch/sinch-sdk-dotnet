using System.Text;

namespace Sinch.Voice.Applications.QueryNumber
{
    public sealed class QueryNumberResponse
    {
        /// <summary>
        ///     The type of method.
        /// </summary>
        public string? Method { get; set; }


        /// <summary>
        ///     Gets or Sets Number
        /// </summary>
        public NumberItem? Number { get; set; }


        /// <summary>
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class QueryNumberResponse {\n");
            sb.Append("  Method: ").Append(Method).Append("\n");
            sb.Append("  Number: ").Append(Number).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }
    }
}
