using System.Text;

namespace Sinch.Voice.Calls.Instructions
{
    /// <summary>
    ///     Forces the callee to answer the call.
    /// </summary>
    public class Answer : IInstruction
    {
        public string Name { get; } = "answer";
        
        /// <summary>
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class Answer {\n");
            sb.Append("  Name: ").Append(Name).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }
    }
}
