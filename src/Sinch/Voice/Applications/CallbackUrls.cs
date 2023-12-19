using System.Text;

namespace Sinch.Voice.Applications
{
    public class Callbacks
    {
        public CallbackUrls Url { get; set; }
    }
    
    /// <summary>
    ///     Gets primary and if configured fallback callback URLs
    /// </summary>
    public class CallbackUrls
    {
        /// <summary>
        ///     Your primary callback URL
        /// </summary>
        public string Primary { get; set; }


        /// <summary>
        ///     Your fallback callback URL (returned if configured). It is used only if Sinch platform gets a timeout or error from
        ///     your primary callback URL.
        /// </summary>
        public string Fallback { get; set; }


        /// <summary>
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class CallbackUrls {\n");
            sb.Append("  Primary: ").Append(Primary).Append("\n");
            sb.Append("  Fallback: ").Append(Fallback).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }
    }
}
