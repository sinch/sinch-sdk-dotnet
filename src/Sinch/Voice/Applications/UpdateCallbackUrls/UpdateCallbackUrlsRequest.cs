using System.Text;

namespace Sinch.Voice.Applications.UpdateCallbackUrls
{
    public sealed class UpdateCallbackUrlsRequest
    {
        /// <summary>
        ///     The unique identifying key of the application.
        /// </summary>

        public required string ApplicationKey { get; set; }

        /// <summary>
        ///     Gets or Sets Url
        /// </summary>
        public CallbackUrls? Url { get; set; }


        /// <summary>
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class UpdateCallbackUrlsRequest {\n");
            sb.Append("  Url: ").Append(Url).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }
    }
}
