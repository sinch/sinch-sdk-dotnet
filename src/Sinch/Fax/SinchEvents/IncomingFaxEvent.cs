using System.Text.Json.Serialization;
using Sinch.Fax.Faxes;

namespace Sinch.Fax.SinchEvents
{
    public sealed class IncomingFaxEvent : FaxSinchEventBase
    {
        public override FaxEventType Event { get; } = FaxEventType.IncomingFax;

        /// <summary>
        ///     Base64 encoded file content.
        /// </summary>
        [JsonPropertyName("file")]
        public string? File { get; set; }

        [JsonPropertyName("fileType")]
        public FileType? FileType { get; set; }
    }
}
