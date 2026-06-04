using System.Text;

namespace Sinch.Voice.Applications.UpdateEventDestinations
{
    public sealed class UpdateEventDestinationsRequest
    {
        /// <summary>
        ///     The unique identifying key of the application.
        /// </summary>
        public required string ApplicationKey { get; set; }

        /// <summary>
        ///     The event destination URLs to configure.
        /// </summary>
        public EventDestinationTarget? Url { get; set; }

        /// <inheritdoc />
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class UpdateEventDestinationsRequest {\n");
            sb.Append("  Url: ").Append(Url).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }
    }
}
