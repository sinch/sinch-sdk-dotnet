using System.Collections.Generic;
using System.Text;
using Sinch.Voice.Calls.Actions;
using Sinch.Voice.Calls.Instructions;

namespace Sinch.Voice.Calls.Manage
{
    /// <summary>
    ///     SVAML is a call control markup language. When a server receives a callback event from the Sinch platform, it can
    ///     respond with a SVAML object to control the voice call. The following is an example of a SVAML object type and its
    ///     contents.
    /// </summary>
    public sealed class ManageWithCallLegRequest
    {
        /// <summary>
        ///     The collection of instructions that can perform various tasks during the call. You can include as many instructions as necessary.
        /// </summary>
        public List<IInstruction> Instructions { get; set; }


        /// <summary>
        ///     Gets or Sets Action
        /// </summary>
        public IAction Action { get; set; }


        /// <summary>
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class ManageWithCallLegRequest: {\n");
            sb.Append("  Instructions: ").Append(Instructions).Append("\n");
            sb.Append("  Action: ").Append(Action).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }
    }
}
