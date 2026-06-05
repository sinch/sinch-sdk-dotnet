using System.Text;

namespace Sinch.Voice.Applications
{
    /// <summary>
    ///     Event destinations configured for a Voice application.
    /// </summary>
    public sealed class EventDestinations
    {
        /// <summary>
        ///     The event destination URLs.
        /// </summary>
        public EventDestinationTarget? Url { get; set; }
    }

    /// <summary>
    ///     Primary and optional fallback event destination URLs for a Voice application.
    /// </summary>
    public sealed class EventDestinationTarget
    {
        /// <summary>
        ///     Your primary event destination URL.
        /// </summary>
        public string? Primary { get; set; }

        /// <summary>
        ///     Your fallback event destination URL (returned if configured). It is used only if the Sinch platform
        ///     gets a timeout or error from your primary event destination URL.
        /// </summary>
        public string? Fallback { get; set; }

        /// <inheritdoc />
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class EventDestinationTarget {\n");
            sb.Append("  Primary: ").Append(Primary).Append("\n");
            sb.Append("  Fallback: ").Append(Fallback).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }
    }
}
