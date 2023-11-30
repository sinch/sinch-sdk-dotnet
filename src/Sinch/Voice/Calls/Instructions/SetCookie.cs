using System.Text;

namespace Sinch.Voice.Calls.Instructions
{
    /// <summary>
    ///     Creates a cookie for the duration of the call.
    /// </summary>
    public class SetCookie : IInstruction
    {
        public string Name { get; } = "setCookie";

        /// <summary>
        ///     The name of the cookie you want to set.
        /// </summary>
        public string Key { get; set; }


        /// <summary>
        ///     The value of the cookie you want to set.
        /// </summary>
        public string Value { get; set; }


        /// <summary>
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class SetCookie {\n");
            sb.Append("  Name: ").Append(Name).Append("\n");
            sb.Append("  Key: ").Append(Key).Append("\n");
            sb.Append("  Value: ").Append(Value).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }
    }
}
